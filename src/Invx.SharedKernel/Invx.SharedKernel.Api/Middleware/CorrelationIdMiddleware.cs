﻿//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;

//namespace Invx.SharedKernel.Api.Middleware;
//public sealed class CorrelationIdMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<CorrelationIdMiddleware> _logger;
//    private const string CorrelationIdHeader = "X-Correlation-ID";

//    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        var correlationId = GetOrCreateCorrelationId(context);

//        using (_logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
//        {
//            context.Response.Headers.Append(CorrelationIdHeader, correlationId);
//            await _next(context);
//        }
//    }

//    private static string GetOrCreateCorrelationId(HttpContext context)
//    {
//        if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId)
//            && !string.IsNullOrEmpty(correlationId))
//        {
//            return correlationId.ToString();
//        }

//        return Guid.NewGuid().ToString();
//    }
//}