using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Invx.SharedKernel.Application.Behaviors;
internal sealed class TransactionBehavior<TRequest, TResponse>(
    IServiceProvider serviceProvider,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetService<DbContext>();

        if (dbContext is null)
        {
            return await next(cancellationToken);
        }

        // Skip if transaction already exists
        if (dbContext.Database.CurrentTransaction is not null)
        {
            return await next(cancellationToken);
        }

        var stopwatch = Stopwatch.StartNew();
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            logger.LogDebug("Starting new transaction for request {RequestType}", typeof(TRequest).Name);

            var response = await next(cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            stopwatch.Stop();
            logger.LogDebug("Transaction committed for {RequestType} in {Elapsed}ms",
                typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "Transaction rolled back for {RequestType} after {Elapsed}ms",
                typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);

            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}