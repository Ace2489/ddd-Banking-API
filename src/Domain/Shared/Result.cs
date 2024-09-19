namespace Domain.Shared;

public record Result
{
    protected private Result(bool isSuccess, Error? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    public static Result Success => new(true);

    public static Result Failure(Error error) => new(false, error);

    public static implicit operator Result(Error error) => Failure(error);
}
