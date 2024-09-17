
namespace Domain.Shared;

public class Result<T> : Result
{
    protected private Result(bool isSuccess, T? value, Error? error = null) : base(isSuccess, error)
    {
        Value = value;
    }
    public T? Value { get; }

    public static new Result<T> Success(T value) => new(true, value);
    public static new Result<T> Failure(Error error) => new(false, default, error);

    public static implicit operator Result<T>(Error error) => Failure(error);

    public static implicit operator Result<T>(T value) => Success(value);

}
