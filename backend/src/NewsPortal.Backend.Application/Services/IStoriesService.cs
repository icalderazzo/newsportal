using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;

namespace NewsPortal.Backend.Application.Services;

public interface IStoriesService : IItemService<StoryDto>
{
    /// <summary>
    ///     Gets the newest stories filtered by pages.
    /// </summary>
    /// <param name="paginationFilter">The pagination filter.</param>
    /// <returns>A PagedResponse of StoryDtos containing the filtered data and the total record count.</returns>
    Task<PagedResponse<List<StoryDto>>> GetNewestStories(PaginationFilter? paginationFilter = null);

    /// <summary>
    ///     Searches stories with a title that contains the search string.    
    /// </summary>
    /// <param name="searchString">The search string.</param>
    /// <param name="paginationFilter">The pagination filter.</param>
    /// <returns>A PagedResponse of StoryDtos containing the filtered data and the total record count.</returns>
    Task<PagedResponse<List<StoryDto>>> Search(string searchString, PaginationFilter paginationFilter);
}