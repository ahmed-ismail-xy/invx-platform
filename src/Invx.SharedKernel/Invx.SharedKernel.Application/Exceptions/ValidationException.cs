using Invx.SharedKernel.Application.Errors;

namespace Invx.SharedKernel.Application.Exceptions;
public sealed class ValidationException(ValidationError error) : Exception(error.Description)
{
    public ValidationError Error { get; } = error;
}