//using Microsoft.Extensions.Diagnostics.HealthChecks;

//namespace Invx.SharedKernel.Api.HealthChecks;
//public class DatabaseHealthCheck : IHealthCheck
//{
//    private readonly string _connectionString;

//    public DatabaseHealthCheck(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<HealthCheckResult> CheckHealthAsync(
//        HealthCheckContext context,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            Implement your database connectivity check here
//             This is a placeholder - replace with actual database connectivity test
//            await Task.Delay(100, cancellationToken);

//            return HealthCheckResult.Healthy("Database is healthy");
//        }
//        catch (Exception ex)
//        {
//            return HealthCheckResult.Unhealthy("Database is unhealthy", ex);
//        }
//    }
//}
