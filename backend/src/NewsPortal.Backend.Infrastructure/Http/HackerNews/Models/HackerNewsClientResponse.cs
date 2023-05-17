using System.Net;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;

public class HackerNewsClientResponse<T>
{
    public HttpStatusCode Code { get; set; }
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
}