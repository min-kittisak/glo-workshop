namespace IdentityWorkshop.Api.Middleware;

public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId) || correlationId.Length > 100)
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        context.Response.Headers[HeaderName] = correlationId;
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            [HeaderName] = correlationId
        });

        await _next(context);
    }
}

