using System.Collections.Concurrent;
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
        //  Items concurrent collection
        var items = new ConcurrentQueue<ItemDto>();

        //  Execute get or create async in parallel 
        await Parallel.ForEachAsync(itemIds ,async(itemId, cancellationToken) =>
        {
            //  Get Item from cache or call the API
            var item = await _memoryCache.GetOrCreateAsync<ItemDto>(
                itemId,
                async entry =>
                {
                    //  Get Item by its id
                    var createItemResult = await createItemFunc(itemId);
                    if (createItemResult is not null)
                    {
                        //  Add it to the cache for 10 min if the request succeeded
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                        return createItemResult;
                    }
                    
                    //  Add an empty object with zero life time if request failed
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.Zero;
                    return new ItemDto();
                });
            
            //  Add item to the queue
            if (item is not null && item.Id > 0) items.Enqueue(item);
        });
        
        return items.ToList();
    }
}