using NewsPortal.Backend.Contracts.Dtos;
using NewsPortal.Backend.Contracts.Filters;

namespace NewsPortal.Backend.Application.Services;

public interface IItemService
{
    /// <summary>
    ///     Gets the newest stories filtered by pages.
    /// </summary>
    /// <param name="paginationFilter">The pagination filter.</param>
    /// <returns>A tuple with the filtered items and the total item count.</returns>
    Task<(List<ItemDto>, int)> GetNewestStories(PaginationFilter? paginationFilter = null);

    /// <summary>
    ///     Searches news with a title that contains the search string.    
    /// </summary>
    /// <param name="searchString">The search string.</param>
    /// <param name="paginationFilter">The pagination filter.</param>
    /// <returns></returns>
    Task<(List<ItemDto>, int)> SearchNews(string searchString, PaginationFilter paginationFilter);
}