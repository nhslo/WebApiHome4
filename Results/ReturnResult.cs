namespace WebApiHome4.Results;

public class ReturnResult<T>
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }

    public static ReturnResult<T> Ok(T data, string message = "Операция выполнена успешно.") =>
        new() { Success = true, Message = message, Data = data };

    public static ReturnResult<T> Failure(string message) =>
        new() { Success = false, Message = message, Data = default };
}
