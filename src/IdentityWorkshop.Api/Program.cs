using IdentityWorkshop.Api.Middleware;
using IdentityWorkshop.Application;
using IdentityWorkshop.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddWorkshopApplication();

var connectionString = builder.Configuration.GetConnectionString("WorkshopDb")
    ?? throw new InvalidOperationException(
        // 5.8.2.2 / LAB 2: จงแก้ ConnectionStrings__WorkshopDb ให้ชี้ชุดทดลองของผู้สอน
        "ConnectionStrings:WorkshopDb is required. Set ConnectionStrings__WorkshopDb before starting the Workshop API.");

builder.Services.AddWorkshopInfrastructure(connectionString);

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new
        {
            error = statusCode == StatusCodes.Status400BadRequest ? exception?.Message : "เกิดข้อผิดพลาดในชุด Workshop",
            correlationId = context.TraceIdentifier
        });
    });
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "identity-workshop",
    utc = DateTime.UtcNow
}));

app.MapControllers();
app.Run();

public partial class Program;
