namespace NewsPortal.Backend.Contracts.Responses;

public class Response<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
}