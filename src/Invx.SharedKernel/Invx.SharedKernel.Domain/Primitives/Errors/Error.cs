using Ardalis.GuardClauses;

namespace Invx.SharedKernel.Domain.Primitives.Errors;
public record Error
{
    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }
    public string? Source { get; init; }
    public Dictionary<string, object>? Metadata { get; init; }

    protected Error(
        string code,
        string description,
        ErrorType type,
        string? source = null,
        Dictionary<string, object>? metadata = null)
    {
        Code = Guard.Against.NullOrEmpty(code);
        Description = Guard.Against.NullOrEmpty(description);
        Type = type;
        Source = source;
        Metadata = metadata;
    }

    public static readonly Error None = new(
        string.Empty,
        string.Empty,
        ErrorType.None);

    public static Error Create(
        ErrorType errorType,
        string code,
        string description,
        string? source = null,
        Dictionary<string, object>? metadata = null)
        => new(code, description, errorType, source, metadata);
}
