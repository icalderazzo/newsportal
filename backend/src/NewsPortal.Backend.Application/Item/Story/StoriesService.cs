using AutoMapper;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.Application.Item.Story;

internal class StoriesService : IStoriesService
{
    private readonly IHackerNewsClient _hackerNewsClient;
    private readonly IItemsCacheService _itemsCacheService;
    private readonly IMapper _mapper;

    public StoriesService(
        IHackerNewsClient hackerNewsClient,  
        IItemsCacheService itemsCacheService,
        IMapper mapper)
    {
        _hackerNewsClient = hackerNewsClient;
        _itemsCacheService = itemsCacheService;
        _mapper = mapper;
    }

    public async Task<(List<StoryDto>, int)> GetNewestStories(PaginationFilter? paginationFilter = null)
    {
        //  Get new story ids
        var newStoriesResponse = await _hackerNewsClient.GetNewStories();
        var newStories = newStoriesResponse.Data!;
        
        //  Apply pagination if filter has been passed
        if (paginationFilter is not null)
            newStories = newStories
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();
        
        //  Get stories from cache service
        var items = await _itemsCacheService.GetOrCreateItems(newStories, GetItemById);

        //  Order stories list by newest
        items = items.OrderByDescending(x => x.Id).ToList();
        
        return new ValueTuple<List<StoryDto>, int>(items, newStoriesResponse.Data!.Count);
    }

    public async Task<(List<StoryDto>, int)> Search(string searchString, PaginationFilter paginationFilter)
    {
        //  Get new story ids
        var newStoriesResponse = await _hackerNewsClient.GetNewStories();

        //  Get items from cache service
        var items = await _itemsCacheService.GetOrCreateItems(newStoriesResponse.Data!, GetItemById);
        
        //  Filter items where title contains the search string
        //  Paginate the result
        items = items
            .Where(i => i.Title.ToLower().Trim().Contains(searchString.Trim().ToLower()))
            .OrderByDescending(i => i.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToList();
        
        return new ValueTuple<List<StoryDto>, int>(items, items.Count);
    }

    /// <summary>
    ///     Calls GetItemById from HackerNews client and returns the Story if request succeeded.
    /// </summary>
    /// <param name="id">The id of the item</param>
    /// <returns>A nullable StoryDto.</returns>
    private async Task<StoryDto?> GetItemById(int id)
    {
        StoryDto? result = null;
        
        var itemResponse = await _hackerNewsClient.GetItemById(id);
        if (itemResponse.Success) result = _mapper.Map<StoryDto>(itemResponse.Data);

        return result;
    }
}