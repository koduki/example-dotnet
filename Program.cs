// 必要なライブラリやモデルの名前空間を読み込みます
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// 1. アプリケーションビルダーの作成
var builder = WebApplication.CreateBuilder(args);


// 2. サービスコンテナへのサービスの登録 (依存性の注入)
// ----------------------------------------------------

// Entity Framework Core の DbContext を登録します
// データベースプロバイダーとしてインメモリデータベースを使用するよう設定
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

// コントローラーベースのAPIを有効にするために必要なサービスを登録します
builder.Services.AddControllers();


// 3. アプリケーションのビルド
var app = builder.Build();


// 4. HTTPリクエストパイプラインの設定 (ミドルウェアの構成)
// ----------------------------------------------------

// 認証・認可ミドルウェアを有効にします
app.UseAuthorization();

// URLとコントローラーのアクションをマッピングします
app.MapControllers();


// 5. アプリケーションの実行
app.Run();