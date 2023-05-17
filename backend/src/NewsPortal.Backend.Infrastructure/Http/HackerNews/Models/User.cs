namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public int Karma { get; set; }
    public string? About { get; set; }
    public List<int>? Submitted { get; set; }
}