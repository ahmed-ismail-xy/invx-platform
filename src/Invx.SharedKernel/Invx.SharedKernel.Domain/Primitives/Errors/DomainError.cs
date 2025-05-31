namespace Invx.SharedKernel.Domain.Primitives.Errors;
public record DomainError : Error
{
    private DomainError(
        string code,
        string description,
        string? source = null,
        Dictionary<string, object>? metadata = null)
        : base(code, description, ErrorType.Domain, source, metadata) { }

    public static DomainError Create(
        string code,
        string description,
        string? source = null,
        Dictionary<string, object>? metadata = null)
        => new(code, description, source, metadata);

    public static DomainError BusinessRuleViolation(
        string rule,
        string description,
        Dictionary<string, object>? metadata = null)
        => new($"Domain.BusinessRule.{rule}", description, "Domain", metadata);

    public static DomainError InvalidOperation(
        string operation,
        string description,
        Dictionary<string, object>? metadata = null)
        => new($"Domain.InvalidOperation.{operation}", description, "Domain", metadata);
}