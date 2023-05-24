using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;

namespace NewsPortal.Backend.Application.Item;

public class ItemsCacheService : IItemsCacheService
{
    private readonly IMemoryCache _memoryCache;
    
    public ItemsCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public async Task<List<T>> GetOrCreateItems<T>(
        IEnumerable<int> itemIds, Func<int, Task<T?>> createItemFunc) where T : ItemDto
    { 
        //  Items concurrent collection
        var items = new ConcurrentBag<T>();

        //  Execute get or create async in parallel 
        await Parallel.ForEachAsync(itemIds ,async(itemId, cancellationToken) =>
        {
            //  Get Item from cache or call the API
            var item = await _memoryCache.GetOrCreateAsync<T>(
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
                    return default!;
                });
        
            //  Add item to the queue
            if (item is not null && item.Id > 0) items.Add(item);
        });
    
        return items.ToList();
    }

    public async Task UpdateItems<T>(
        IEnumerable<int> itemIds, Func<int, Task<T?>> getUpdatedItemFunc) where T : ItemDto
    {
        await Parallel.ForEachAsync(itemIds,async (itemId, cancellationToken) =>
        {
            var item = _memoryCache.Get<T>(itemId);
            if (item is not null)
            {
               var updatedItem = await getUpdatedItemFunc(itemId);
               if (updatedItem is not null)
               {
                   _memoryCache.Remove(itemId);
                   _memoryCache.Set(itemId, updatedItem);
               }
            }
        });
    }
}