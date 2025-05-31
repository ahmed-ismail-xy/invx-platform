using System.Text;

namespace Invx.SharedKernel.Domain.Primitives.Errors;
public static class ErrorExtensions
{
    public static Error WithMetadata(this Error error, string key, object value)
    {
        var metadata = error.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
        metadata[key] = value;

        return error with { Metadata = metadata };
    }

    public static Error WithMetadata(this Error error, Dictionary<string, object> additionalMetadata)
    {
        var metadata = error.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];

        foreach (var kvp in additionalMetadata)
        {
            metadata[kvp.Key] = kvp.Value;
        }

        return error with { Metadata = metadata };
    }

    public static Error WithSource(this Error error, string source)
        => error with { Source = source };

    public static Error WithTenantContext(this Error error, Guid tenantId, string? tenantName = null)
    {
        var metadata = error.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
        metadata["TenantId"] = tenantId;
        if (!string.IsNullOrEmpty(tenantName))
            metadata["TenantName"] = tenantName;

        return error with { Metadata = metadata };
    }

    public static Error WithUserContext(this Error error, Guid userId, string? userName = null)
    {
        var metadata = error.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
        metadata["UserId"] = userId;
        if (!string.IsNullOrEmpty(userName))
            metadata["UserName"] = userName;

        return error with { Metadata = metadata };
    }

    public static Error WithCorrelationId(this Error error, string correlationId)
        => error.WithMetadata("CorrelationId", correlationId);

    public static bool IsOfType<TError>(this Error error) where TError : Error
        => error is TError;

    public static TError? AsType<TError>(this Error error) where TError : Error
        => error as TError;

    public static string ToDetailedString(this Error error)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Code: {error.Code}");
        sb.AppendLine($"Description: {error.Description}");
        sb.AppendLine($"Type: {error.Type}");

        if (!string.IsNullOrEmpty(error.Source))
            sb.AppendLine($"Source: {error.Source}");

        if (error.Metadata?.Any() == true)
        {
            sb.AppendLine("Metadata:");
            foreach (var kvp in error.Metadata)
            {
                sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        return sb.ToString();
    }
}