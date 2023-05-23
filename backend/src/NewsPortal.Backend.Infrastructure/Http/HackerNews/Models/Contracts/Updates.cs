namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.Models.Contracts;

public class Updates
{
    public List<int> Items { get; set; } = new();
    public List<string> Profiles { get; set; } = new();
}