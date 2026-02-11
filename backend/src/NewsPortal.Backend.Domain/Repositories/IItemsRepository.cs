using NewsPortal.Backend.Domain.Filters;
using NewsPortal.Backend.Domain.Models.Items;

namespace NewsPortal.Backend.Domain.Repositories;

public interface IItemsRepository : IRepository<Item>
{
    Task<UserItem> BookmarkItem(UserItem userItem);
    Task<List<UserItem>> GetBookmarkItems(int userId, DbPaginationFilter? paginationFilter = null);
    Task<int> CountBookmarkItems(int userId);
}