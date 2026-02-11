using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Domain.Models.Items;
using NewsPortal.Backend.Domain.Repositories;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.Application.Item;

internal abstract class BaseItemService<TDomain, TDto> : IItemService<TDomain, TDto>     
    where TDomain : Domain.Models.Items.Item 
    where TDto : ItemDto
{
    protected readonly IHackerNewsClient HackerNewsClient;
    protected readonly IItemsRepository ItemsRepository;
    protected readonly IItemsCacheService ItemsCacheService;
    protected readonly ItemMapper Mapper;
    
    protected BaseItemService(
        IHackerNewsClient hackerNewsClient, 
        IItemsRepository itemsRepository,
        IItemsCacheService itemsCacheService,
        ItemMapper mapper)
    {
        HackerNewsClient = hackerNewsClient;
        ItemsRepository = itemsRepository;
        ItemsCacheService = itemsCacheService;
        Mapper = mapper;
    }
    
    public async Task UpdateItems()
    {
        var updatedItems = await HackerNewsClient.GetChangedItemsAndProfiles();
        if(updatedItems.Data is not null)
            await ItemsCacheService.UpdateItems(updatedItems.Data.Items, GetItemById);
    }

    public async Task BookmarkItem(int itemId)
    {
        // Check if item has been already saved on DB
        var itemExistsOnDb = await ItemsRepository.Exists(itemId);
        if (!itemExistsOnDb)
        {
            // Get item from cache (or API call) & map to domain model
            var item = await GetItemById(itemId);
            var domainItem = Mapper.MapToDomainItem<TDomain>(item!);
            
            // Save item
            await ItemsRepository.Save(domainItem);
        }
        
        // Bookmark item for current user
        await ItemsRepository.BookmarkItem(new UserItem()
        {
            ItemId = itemId,
            UserId = 1,
        });
    }

    /// <summary>
    ///     Calls GetItemById from HackerNews client and returns the mapped item if request succeeded.
    /// </summary>
    /// <param name="itemId">The item id.</param>
    /// <returns></returns>
    protected async Task<TDto?> GetItemById(int itemId)
    {
        TDto? result = null;
        
        var itemResponse = await HackerNewsClient.GetItemById(itemId);
        if (itemResponse.Success && itemResponse.Data != null) 
            result = Mapper.MapToItemDto<TDto>(itemResponse.Data);

        return result;
    }
}