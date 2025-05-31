using System.Diagnostics;
using Invx.SharedKernel.Domain.Primitives.Results;
using Microsoft.Extensions.Logging;

namespace Invx.SharedKernel.Application.Behaviors;
public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("[START] Handling {Request} ({@Request})", requestName, request);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next(cancellationToken);

            stopwatch.Stop();

            if (response is Result result)
            {
                if (result.IsSuccess)
                    logger.LogInformation(
                        "[END] {Request} succeeded in {ElapsedMs} ms",
                        requestName,
                        stopwatch.ElapsedMilliseconds);
                else
                    logger.LogError(
                        "[END] {Request} failed: {Error} in {ElapsedMs} ms",
                        requestName,
                        result.Error,
                        stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogInformation(
                    "[END] {Request} handled in {ElapsedMs} ms with response {@Response}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    response);
            }

            if (stopwatch.ElapsedMilliseconds > 3000)
            {
                logger.LogWarning(
                    "[PERFORMANCE] {Request} took too long: {ElapsedMs} ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(
                ex,
                "[ERROR] {Request} failed after {ElapsedMs} ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}