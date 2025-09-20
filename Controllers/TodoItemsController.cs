using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models; // ← あとで作成するモデルクラスの名前空間

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")] // URLは "api/TodoItems" になります
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    // データベースコンテキストをコンストラクターで受け取ります (依存性の注入)
    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/TodoItems
    // 全件取得
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        return await _context.TodoItems.ToListAsync();
    }

    // GET: api/TodoItems/5
    // IDによる個別取得
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            // アイテムが見つからなければ 404 Not Found を返す
            return NotFound();
        }

        return todoItem;
    }

    // POST: api/TodoItems
    // 新規作成
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        // CreatedAtAction を使い、ステータスコード 201 Created を返す
        // レスポンスの Location ヘッダーに、作成されたアイテムのURLを含める
        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
    }

    // PUT: api/TodoItems/5
    // 更新
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            // URLのIDとボディのIDが一致しなければ 400 Bad Request を返す
            return BadRequest();
        }

        _context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        
        // 成功したら 204 No Content を返す (ボディは空)
        return NoContent();
    }


    // DELETE: api/TodoItems/5
    // 削除
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        // 成功したら 204 No Content を返す
        return NoContent();
    }
    
    // IDの存在チェック用プライベートメソッド
    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
}