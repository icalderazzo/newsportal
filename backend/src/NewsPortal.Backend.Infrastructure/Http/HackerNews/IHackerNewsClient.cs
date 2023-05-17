using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models.Contracts;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews;

public interface IHackerNewsClient
{
    Task<HackerNewsClientResponse<Item>> GetItemById(int itemId);
    Task<HackerNewsClientResponse<List<int>>> GetTopStories();
    Task<HackerNewsClientResponse<List<int>>> GetNewStories();
}