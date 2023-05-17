using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.Application.Item;

internal class ItemService : IItemService
{
    private readonly IHackerNewsClient _hackerNewsClient;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public ItemService(
        IHackerNewsClient hackerNewsClient,  
        IMemoryCache memoryCache, 
        IMapper mapper)
    {
        _hackerNewsClient = hackerNewsClient;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async Task<(List<ItemDto>, int)> GetNewestStories(PaginationFilter paginationFilter)
    {
        //  Get new story ids
        var newStoriesResponse = await _hackerNewsClient.GetNewStories();
        
        //  Order and page ids
        var newStories = newStoriesResponse.Data!
            .OrderByDescending(x => x)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToList();

        var items = new List<ItemDto>();
        
        //  Execute get or create async in parallel 
        await Parallel.ForEachAsync(newStories ,async(storyId, cancellationToken) =>
        {
            //  Get Item from cache or call the API
            var story = await _memoryCache.GetOrCreateAsync<ItemDto>(
                storyId,
                async entry =>
                {
                    //  Get Item by its id
                    var itemResponse = await _hackerNewsClient.GetItemById(storyId);
                    if (itemResponse.Success)
                    {
                        //  Add it to the cache for 10 min if the request succeeded
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                        return _mapper.Map<ItemDto>(itemResponse.Data);
                    }
                    
                    //  Add an empty object with zero life time if request failed
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.Zero;
                    return new ItemDto();
                });
            
            //  Make resource thread safe
            lock (items) if (story != null) items.Add(story);
        });
        
        return new ValueTuple<List<ItemDto>, int>(items, newStoriesResponse.Data!.Count);
    }
}