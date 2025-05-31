namespace Invx.SharedKernel.Application.Errors;
public record ApplicationError : Error
{
    private ApplicationError(
        string code,
        string description,
        ErrorType type,
        Dictionary<string, object>? metadata = null)
        : base(
            code,
            description,
            type,
            "Application",
            metadata) { }

    public static ApplicationError NotFound(
        string resource,
        string identifier,
        Dictionary<string, object>? metadata = null)
        => new(
            $"Application.{resource}.NotFound",
            $"{resource} with identifier '{identifier}' was not found.",
            ErrorType.NotFound,
            metadata);

    public static ApplicationError Conflict(
        string resource,
        string reason,
        Dictionary<string, object>? metadata = null)
        => new($"Application.{resource}.Conflict", reason, ErrorType.Conflict, metadata);

    public static ApplicationError Unauthorized(
        string action,
        Dictionary<string, object>? metadata = null)
        => new(
            "Application.Unauthorized",
            $"Not authorized to perform action: {action}",
            ErrorType.Unauthorized,
            metadata);
}
