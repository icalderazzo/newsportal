using AutoMapper;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.Application.Item;

internal class ItemService : IItemService
{
    private readonly IHackerNewsClient _hackerNewsClient;
    private readonly IItemsCacheService _itemsCacheService;
    private readonly IMapper _mapper;

    public ItemService(
        IHackerNewsClient hackerNewsClient,  
        IItemsCacheService itemsCacheService,
        IMapper mapper)
    {
        _hackerNewsClient = hackerNewsClient;
        _itemsCacheService = itemsCacheService;
        _mapper = mapper;
    }

    public async Task<(List<ItemDto>, int)> GetNewestStories(PaginationFilter? paginationFilter = null)
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
        
        //  Get items from cache service
        var items = await _itemsCacheService.GetOrCreateItems(newStories, GetItemById);

        //  Order stories list by newest
        items = items.OrderByDescending(x => x.Id).ToList();
        
        return new ValueTuple<List<ItemDto>, int>(items, newStoriesResponse.Data!.Count);
    }

    public async Task<(List<ItemDto>, int)> SearchNews(string searchString, PaginationFilter paginationFilter)
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
        
        return new ValueTuple<List<ItemDto>, int>(items, items.Count);
    }

    /// <summary>
    ///     Calls GetItemById from HackerNews client and returns the object if request succeeded.
    /// </summary>
    /// <param name="id">The id of the item</param>
    /// <returns>A nullable ItemDto.</returns>
    private async Task<ItemDto?> GetItemById(int id)
    {
        ItemDto? result = null;
        
        var itemResponse = await _hackerNewsClient.GetItemById(id);
        if (itemResponse.Success) result = _mapper.Map<ItemDto>(itemResponse.Data);

        return result;
    }
}