using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;

namespace NewsPortal.Backend.Application.Services;

public interface IItemService<TDomain, TDto>
    where TDomain : Domain.Models.Items.Item
    where TDto : ItemDto
{
    Task UpdateItems();
    Task BookmarkItem(int itemId);
    Task<PagedResponse<List<TDto>>> GetBookmarks(PaginationFilter? filter = null);
    Task DeleteBookmark(int itemId);
}