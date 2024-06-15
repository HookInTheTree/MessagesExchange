using Microsoft.AspNetCore.Diagnostics;

namespace MessagesExchange.Infrastructure.Logging;

public class ExceptionsHandler : IExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionsHandler> _logger;

    public ExceptionsHandler(RequestDelegate next, ILogger<ExceptionsHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        if (httpContext.Request.Path.HasValue && !httpContext.Request.Path.Value.Contains("api"))
        {
            httpContext.Response.Redirect("/error");
        }

        return new ValueTask<bool>(true);
    }

}
