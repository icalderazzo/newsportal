using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.Domain.Models.Items;

namespace NewsPortal.Backend.Application.Services;

public interface IStoriesService : IItemService<Story, StoryDto>
{
    /// <summary>
    ///     Gets the newest stories filtered by pages.
    /// </summary>
    /// <param name="paginationFilter">The pagination filter. This is optional in order to allow getting all stories at once.</param>
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