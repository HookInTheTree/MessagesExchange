namespace MessagesExchange.Infrastructure.Logging;

public class RequestsLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestsLoggingMiddleware> logger;

    public RequestsLoggingMiddleware(RequestDelegate _next, ILogger<RequestsLoggingMiddleware> _logger)
    {
        next = _next;
        logger = _logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);
        await LogResponse(context);
    }

    private Task LogResponse(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
