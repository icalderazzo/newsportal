using NewsPortal.Backend.Contracts.Dtos.Item;

namespace NewsPortal.Backend.Application.Services;

public interface IItemsCacheService
{
    /// <summary>
    ///     Gets specified items from the cache or adds them if they do not exist. 
    /// </summary>
    /// <param name="itemIds">The specified items id list.</param>
    /// <param name="createItemFunc">Delegate to create items in case they're not present in the cache.</param>
    /// <returns></returns>
    Task<List<T>> GetOrCreateItems<T>(IEnumerable<int> itemIds, Func<int, Task<T?>> createItemFunc) where T : ItemDto;
}