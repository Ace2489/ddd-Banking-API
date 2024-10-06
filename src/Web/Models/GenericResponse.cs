namespace Web.Models;

public record Response<T>
{
    private Response(T? data, bool Success, string message = "")
    {
        Data = data;
        Status = Success == true ? "Success" : "Error";
        Message = message;
    }
    public string Status { get; }

    public string Message { get; }

    public T? Data { get; }

    public static Response<T> Success(T data, string message) => new(data, true, message);
    public static Response<T> Success(T data) => new(data, true);
    public static Response<T> Failure(string message) => new(default, true, message);


}
