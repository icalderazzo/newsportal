using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews;

public interface IHackerNewsClient
{
    Task<Item> GetItemById(int itemId);
    Task<List<int>> GetTopStories();
    Task<List<int>> GetNewStories();
}