namespace Invx.SharedKernel.Domain.Primitives.Results;
public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new ArgumentException("Success result cannot have an error.", nameof(error));

        if (!isSuccess && error == Error.None)
            throw new ArgumentException("Failure result must have an error.", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static implicit operator Result(Error error) => Failure(error);

    public static Result SuccessIf(bool condition, Error error)
        => condition ? Success() : Failure(error);

    public static Result FailureIf(bool condition, Error error)
        => condition ? Failure(error) : Success();
}