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

    /// <summary>
    ///     Updates the specified items in the cache.
    /// </summary>
    /// <param name="itemIds">The specified items id list.</param>
    /// <param name="getUpdatedItemFunc">Delegate to get an updated item.</param>
    /// <typeparam name="T">Type of the item to update.</typeparam>
    /// <returns></returns>
    Task UpdateItems<T>(IEnumerable<int> itemIds, Func<int, Task<T?>> getUpdatedItemFunc) where T : ItemDto;
}