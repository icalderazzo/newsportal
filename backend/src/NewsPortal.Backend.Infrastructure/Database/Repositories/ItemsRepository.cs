using NewsPortal.Backend.Domain.Models.Items;
using NewsPortal.Backend.Domain.Repositories;

namespace NewsPortal.Backend.Infrastructure.Database.Repositories;

internal class ItemsRepository : BaseRepository<Item>, IItemsRepository
{
    public ItemsRepository(NewsPortalContext context) : base(context)
    {
    }

    public async Task<UserItem> BookmarkItem(UserItem userItem)
    {
        var newEntry = Context.Set<UserItem>().Add(userItem).Entity;
        await Complete();
        
        return newEntry;
    }
}