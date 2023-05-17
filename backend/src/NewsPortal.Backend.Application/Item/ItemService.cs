using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos;
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

    public async Task<List<ItemDto>> GetNewestStories()
    {
        var result = new List<ItemDto>();
        
        //  Get new story ids
        var newStories = await _hackerNewsClient.GetNewStories();
        //  Execute get or create async in parallel 
        await Parallel.ForEachAsync(newStories,async(storyId, cancellationToken) =>
        {
            //  Get Item from cache or call the API
            var story = await _memoryCache.GetOrCreateAsync<ItemDto>(
                storyId,
                async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _mapper.Map<ItemDto>(await _hackerNewsClient.GetItemById(storyId));
                });
            
            //  Make resource thread safe
            lock (result) if (story != null) result.Add(story);
        });

        return result;
    }
}