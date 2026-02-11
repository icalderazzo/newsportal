using Microsoft.EntityFrameworkCore;
using NewsPortal.Backend.Domain.Filters;
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

    public async Task<List<UserItem>> GetBookmarkItems(int userId, DbPaginationFilter? paginationFilter = null)
    {
        var query = Context.Set<UserItem>()
            .AsNoTracking()
            .Where(ui => ui.UserId.Equals(userId));

        if (paginationFilter is not null)
        {
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            query = query.Skip(skip).Take(paginationFilter.PageSize);
        }

        query = query.Include(ui => ui.Item);

        return await query.ToListAsync();
    }

    public async Task<int> CountBookmarkItems(int userId)
    {
        return await Context.Set<UserItem>().CountAsync(ui => ui.UserId.Equals(userId));
    }
}