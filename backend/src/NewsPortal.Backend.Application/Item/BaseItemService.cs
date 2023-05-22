using AutoMapper;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.Application.Item;

internal class BaseItemService<T> : IItemService where T : ItemDto
{
    protected readonly IHackerNewsClient HackerNewsClient;
    protected readonly IItemsCacheService ItemsCacheService;
    protected readonly IMapper Mapper;
    
    protected BaseItemService(
        IHackerNewsClient hackerNewsClient, 
        IItemsCacheService itemsCacheService, 
        IMapper mapper)
    {
        HackerNewsClient = hackerNewsClient;
        ItemsCacheService = itemsCacheService;
        Mapper = mapper;
    }
    
    public async Task UpdateItems()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Calls GetItemById from HackerNews client and returns the mapped item if request succeeded.
    /// </summary>
    /// <param name="itemId">The item id.</param>
    /// <returns></returns>
    protected async Task<T?> GetItemById(int itemId)
    {
        T? result = null;
        
        var itemResponse = await HackerNewsClient.GetItemById(itemId);
        if (itemResponse.Success) result = Mapper.Map<T>(itemResponse.Data);

        return result;
    }
}