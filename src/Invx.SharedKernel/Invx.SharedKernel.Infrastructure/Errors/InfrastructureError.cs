namespace Invx.SharedKernel.Infrastructure.Errors;
public record InfrastructureError : Error
{
    private InfrastructureError(
        string code,
        string description,
        ErrorType type,
        Dictionary<string, object>? metadata = null)
        : base(code, description, type, "Infrastructure", metadata) { }

    public static InfrastructureError DatabaseConnectionFailed(
        string connectionString,
        Exception? innerException = null)
        => new("Infrastructure.Database.ConnectionFailed",
               "Failed to establish database connection.",
               ErrorType.Database,
               new Dictionary<string, object>
               {
                   ["ConnectionString"] = MaskConnectionString(connectionString),
                   ["InnerException"] = innerException?.Message ?? "None"
               });

    public static InfrastructureError DatabaseTimeout(string operation, int timeoutSeconds)
        => new("Infrastructure.Database.Timeout",
               $"Database operation '{operation}' timed out after {timeoutSeconds} seconds.",
               ErrorType.Database,
               new Dictionary<string, object> { ["Operation"] = operation, ["TimeoutSeconds"] = timeoutSeconds });

    public static InfrastructureError DatabaseConstraintViolation(string constraint, string details)
        => new("Infrastructure.Database.ConstraintViolation",
               $"Database constraint violation: {constraint}",
               ErrorType.Database,
               new Dictionary<string, object> { ["Constraint"] = constraint, ["Details"] = details });

    public static InfrastructureError ExternalServiceUnavailable(string serviceName, string endpoint)
        => new($"Infrastructure.External.{serviceName}.Unavailable",
               $"External service '{serviceName}' is unavailable.",
               ErrorType.External,
               new Dictionary<string, object> { ["ServiceName"] = serviceName, ["Endpoint"] = endpoint });

    public static InfrastructureError ExternalServiceTimeout(string serviceName, int timeoutMs)
        => new($"Infrastructure.External.{serviceName}.Timeout",
               $"External service '{serviceName}' request timed out after {timeoutMs}ms.",
               ErrorType.External,
               new Dictionary<string, object> { ["ServiceName"] = serviceName, ["TimeoutMs"] = timeoutMs });

    public static InfrastructureError ExternalServiceAuthenticationFailed(string serviceName)
        => new($"Infrastructure.External.{serviceName}.AuthenticationFailed",
               $"Authentication failed for external service '{serviceName}'.",
               ErrorType.External,
               new Dictionary<string, object> { ["ServiceName"] = serviceName });

    public static InfrastructureError NetworkError(string operation, string details)
        => new("Infrastructure.Network.Error",
               $"Network error during '{operation}': {details}",
               ErrorType.Network,
               new Dictionary<string, object> { ["Operation"] = operation, ["Details"] = details });

    public static InfrastructureError FileSystemError(string operation, string path, string details)
        => new("Infrastructure.FileSystem.Error",
               $"File system error during '{operation}' on path '{path}': {details}",
               ErrorType.External,
               new Dictionary<string, object> { ["Operation"] = operation, ["Path"] = path, ["Details"] = details });

    private static string MaskConnectionString(string connectionString)
    {
        // Simple masking - in production
        return connectionString.Length > 10
            ? connectionString[..10] + "***"
            : "***";
    }
}