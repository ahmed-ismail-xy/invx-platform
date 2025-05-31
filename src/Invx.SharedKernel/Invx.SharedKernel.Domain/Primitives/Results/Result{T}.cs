namespace Invx.SharedKernel.Domain.Primitives.Results;
public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(
        TValue? value,
        bool isSuccess,
        Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failed result.");

    public TValueOut Match<TValueOut>(
        Func<TValue, TValueOut> onSuccess,
        Func<Error, TValueOut> onFailure)
        => IsSuccess ? onSuccess(Value) : onFailure(Error);

    public async Task<TValueOut> MatchAsync<TValueOut>(
        Func<TValue, Task<TValueOut>> onSuccess,
        Func<Error, Task<TValueOut>> onFailure)
        => IsSuccess ? await onSuccess(Value) : await onFailure(Error);

    public static implicit operator Result<TValue>(TValue? value)
        => value is not null ? Success(value) : Failure<TValue>(DomainError.Create("Value.Null", "Value cannot be null"));

    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
}