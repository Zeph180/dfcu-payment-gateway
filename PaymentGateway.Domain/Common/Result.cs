namespace PaymentGateway.Domain.Common;

public class Result<T>
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public T? Data { get; private set; }
    
    public static Result<T> Ok(T data) =>
        new Result<T> { Success = true, Data = data };

    public static Result<T> Fail(string message) =>
        new Result<T> { Success = false, Message = message };
}