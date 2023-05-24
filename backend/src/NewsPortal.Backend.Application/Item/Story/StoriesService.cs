using System.Runtime.CompilerServices;
using AutoMapper;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

[assembly: InternalsVisibleTo("NewsPortal.Backend.UnitTests")]
namespace NewsPortal.Backend.Application.Item.Story;

internal class StoriesService : BaseItemService<StoryDto>, IStoriesService 
{
    public StoriesService(
        IHackerNewsClient hackerNewsClient,  
        IItemsCacheService itemsCacheService,
        IMapper mapper) : base(hackerNewsClient, itemsCacheService, mapper)
    {
    }

    public async Task<PagedResponse<List<StoryDto>>> GetNewestStories(PaginationFilter? paginationFilter = null)
    {
        //  Get new story ids
        var newStoriesResponse = await HackerNewsClient.GetNewStories();
        var newStories = newStoriesResponse.Data!;
        
        //  Apply pagination if filter has been passed
        if (paginationFilter is not null)
            newStories = newStories
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();
        
        //  Get stories from cache service
        var items = await ItemsCacheService.GetOrCreateItems(newStories, GetItemById);

        //  Order stories list by newest
        items = items.OrderByDescending(x => x.Id).ToList();

        return new PagedResponse<List<StoryDto>>
        {
            Data = items,
            TotalRecords = newStoriesResponse.Data!.Count
        };
    }

    public async Task<PagedResponse<List<StoryDto>>> Search(string searchString, PaginationFilter paginationFilter)
    {
        //  Get new story ids
        var newStoriesResponse = await HackerNewsClient.GetNewStories();

        //  Get items from cache service
        var items = await ItemsCacheService.GetOrCreateItems(newStoriesResponse.Data!, GetItemById);
        
        //  Filter items where title contains the search string
        var filteredItems = items
            .Where(i => i.Title.Trim().Contains(searchString.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();

        //  Build and return the paginated response
        return new PagedResponse<List<StoryDto>>()
        {
            Data = filteredItems
                .OrderByDescending(i => i.Id)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList(),
            TotalRecords = filteredItems.Count
        };
    }
}