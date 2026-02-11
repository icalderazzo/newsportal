using NewsPortal.Backend.Domain.Models.Items;

namespace NewsPortal.Backend.Domain.Repositories;

public interface IItemsRepository : IRepository<Item>
{
    Task<UserItem> BookmarkItem(UserItem userItem);
}