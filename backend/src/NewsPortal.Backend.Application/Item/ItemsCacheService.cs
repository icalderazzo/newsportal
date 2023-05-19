using Microsoft.Extensions.Caching.Memory;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos;

namespace NewsPortal.Backend.Application.Item;

public class ItemsCacheService : IItemsCacheService
{
    private readonly IMemoryCache _memoryCache;
    
    public ItemsCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public async Task<List<ItemDto>> GetOrCreateItems(
        IEnumerable<int> itemIds, Func<int, Task<ItemDto?>> createItemFunc)
    {
        var items = new List<ItemDto>();

        //  Execute get or create async in parallel 
        await Parallel.ForEachAsync(itemIds ,async(storyId, cancellationToken) =>
        {
            //  Get Item from cache or call the API
            var story = await _memoryCache.GetOrCreateAsync<ItemDto>(
                storyId,
                async entry =>
                {
                    //  Get Item by its id
                    var itemResponse = await createItemFunc(storyId);
                    if (itemResponse is not null)
                    {
                        //  Add it to the cache for 10 min if the request succeeded
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                        return itemResponse;
                    }
                    
                    //  Add an empty object with zero life time if request failed
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.Zero;
                    return new ItemDto();
                });
            
            //  Make resource thread safe
            lock (items) if (story != null) items.Add(story);
        });
        
        return items;
    }
}