namespace Nexel.Domain.Shared;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && !error.Equals(Error.None)) throw new InvalidOperationException();

        if (!isSuccess && error.Equals(Error.None)) throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }

    public static Result<TValue> Failure<TValue>(TValue value)
    {
        return new Result<TValue>(value, false, Error.DefaultError);
    }

    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new Result<TValue>(default, false, error);
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => _value!;

    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }
}