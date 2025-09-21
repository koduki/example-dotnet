using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
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
            .AddAspNetCoreInstrumentation(options =>
            {
                options.EnrichWithHttpRequest = (activity, httpRequest) =>
                {
                    foreach (var header in httpRequest.Headers)
                    {
                        activity.SetTag($"http.request.header.{header.Key.ToLower()}", string.Join(", ", header.Value.ToArray()));
                    }
                };
                options.EnrichWithHttpResponse = (activity, httpResponse) =>
                {
                    foreach (var header in httpResponse.Headers)
                    {
                        activity.SetTag($"http.response.header.{header.Key.ToLower()}", string.Join(", ", header.Value.ToArray()));
                    }
                };
                options.RecordException = true;
            })
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
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation() 
            .AddHttpClientInstrumentation()
            .AddProcessInstrumentation()  
            .AddRuntimeInstrumentation();

        metrics.AddOtlpExporter();

        // 開発環境でのみ、コンソールにもメトリクスを出力する
        if (builder.Environment.IsDevelopment())
        {
            metrics.AddConsoleExporter();
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