namespace Invx.SharedKernel.Application.Errors;
public record ValidationError : Error
{
    public IReadOnlyDictionary<string, string[]> ValidationErrors { get; }

    private ValidationError(
        string code,
        string description,
        IReadOnlyDictionary<string, string[]> validationErrors)
        : base(code, description, ErrorType.Validation, "Application")
    {
        ValidationErrors = validationErrors;
    }

    public static ValidationError Create(IReadOnlyDictionary<string, string[]> validationErrors)
        => new("Application.Validation", "One or more validation errors occurred.", validationErrors);

    public static ValidationError Create(string propertyName, string errorMessage)
        => Create(new Dictionary<string, string[]> { [propertyName] = [errorMessage] });
}