using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Npgsql.Internal;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessagesExchange.Infrastructure.Logging;

public class RequestsExceptionsHandler : IExceptionHandler
{
    private readonly ILogger<RequestsExceptionsHandler> _logger;
    private readonly IHostEnvironment _env;


    public RequestsExceptionsHandler(
        ILogger<RequestsExceptionsHandler> logger,
        IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        if (httpContext.Request.Path.HasValue && !httpContext.Request.Path.Value.Contains("api"))
        {
            httpContext.Response.Redirect("/error");
        }
        else
        {
            var details = ToJson(CreateProblemDetails(httpContext, exception)); 

            const string contentType = "application/problem+json";
            httpContext.Response.ContentType = contentType;
            await httpContext.Response.WriteAsync(details, cancellationToken);
        }

        return true;
    }

    private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
    {
        var statusCode = context.Response.StatusCode;
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
        if (string.IsNullOrEmpty(reasonPhrase))
        {
            reasonPhrase = "An unhandled exception has occurred while executing the request.";
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = reasonPhrase
        };

        if (_env.IsProduction())
        {
            return problemDetails;
        }

        problemDetails.Detail = exception.ToString();
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["data"] = exception.Data;

        return problemDetails;
    }

    private string ToJson(in ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(
                value:problemDetails,
                options:new(JsonSerializerDefaults.Web)
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception has occurred while serializing error to JSON");
            return string.Empty;
        }
    }
}
