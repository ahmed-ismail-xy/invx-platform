using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Invx.SharedKernel.Api.Middleware;
public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        logger.LogInformation("Request started: {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        await next(context);

        var duration = DateTime.UtcNow - startTime;

        logger.LogInformation("Request completed: {Method} {Path} {StatusCode} in {Duration}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            duration.TotalMilliseconds);
    }
}
