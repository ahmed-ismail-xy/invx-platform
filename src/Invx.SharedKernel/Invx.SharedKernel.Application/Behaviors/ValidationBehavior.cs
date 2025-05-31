using Invx.SharedKernel.Application.Errors;

namespace Invx.SharedKernel.Application.Behaviors;
internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = validators
            .Select(validator => validator.Validate(context))
            .Where(result => result.Errors.Count != 0)
            .SelectMany(result => result.Errors);

        var groupedErrors = validationFailures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(f => f.ErrorMessage).ToArray()
            );

        if (groupedErrors.Count != 0)
        {
            throw new Exceptions.ValidationException(ValidationError.Create(groupedErrors));
        }

        return await next(cancellationToken);
    }
}