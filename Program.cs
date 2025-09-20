using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// DIへのサービスの登録
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.SetDbStatementForText = true;
            })
            .AddOtlpExporter(); 

        // 開発環境でのみ、コンソールにもトレースを出力する
        if (builder.Environment.IsDevelopment())
        {
            tracing.AddConsoleExporter();
        }
    });
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

// Appの作成
var app = builder.Build();

// ミドルウェアの設定
app.MapOpenApi();
app.MapControllers();

app.Run();