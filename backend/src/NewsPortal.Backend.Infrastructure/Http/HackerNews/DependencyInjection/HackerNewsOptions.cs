namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.DependencyInjection;

public class HackerNewsOptions
{
    public required string BaseUrl { get; set; }
    public int Version { get; set; }
}