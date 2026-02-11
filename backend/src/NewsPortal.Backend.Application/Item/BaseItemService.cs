using NewsPortal.Backend.Application.Mappers;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.Domain.Models.Items;
using NewsPortal.Backend.Domain.Repositories;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

namespace NewsPortal.Backend.Application.Item;

internal abstract class BaseItemService<TDomain, TDto> : IItemService<TDomain, TDto>
    where TDomain : Domain.Models.Items.Item
    where TDto : ItemDto
{
    private const int TestUserId = 1;
    protected readonly FilterMapper FilterMapper;

    protected readonly IHackerNewsClient HackerNewsClient;
    protected readonly ItemMapper ItemMapper;
    protected readonly IItemsCacheService ItemsCacheService;
    protected readonly IItemsRepository ItemsRepository;

    protected BaseItemService(
        IHackerNewsClient hackerNewsClient,
        IItemsRepository itemsRepository,
        IItemsCacheService itemsCacheService,
        ItemMapper itemMapper,
        FilterMapper filterMapper)
    {
        HackerNewsClient = hackerNewsClient;
        ItemsRepository = itemsRepository;
        ItemsCacheService = itemsCacheService;
        ItemMapper = itemMapper;
        FilterMapper = filterMapper;
    }

    public async Task UpdateItems()
    {
        var updatedItems = await HackerNewsClient.GetChangedItemsAndProfiles();
        if (updatedItems.Data is not null)
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
            var domainItem = ItemMapper.MapToDomainItem<TDomain>(item!);

            // Save item
            await ItemsRepository.Save(domainItem);
        }

        // Bookmark item for current user
        await ItemsRepository.BookmarkItem(new UserItem
        {
            ItemId = itemId,
            UserId = TestUserId
        });
    }

    public async Task<PagedResponse<List<TDto>>> GetBookmarks(PaginationFilter? filter = null)
    {
        var dbFilter = FilterMapper.MapToDomain(filter);
        var bookMarkItems = await ItemsRepository.GetBookmarkItems(TestUserId, dbFilter);

        var bookmarkCount = await ItemsRepository.CountBookmarkItems(TestUserId);

        return new PagedResponse<List<TDto>>
        {
            Data = bookMarkItems.Select(ui => ItemMapper.MapToItemDto<TDto>(ui.Item)).ToList(),
            TotalRecords = bookmarkCount
        };
    }

    public async Task DeleteBookmark(int itemId)
    {
        await ItemsRepository.DeleteBookmark(itemId, TestUserId);
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
            result = ItemMapper.MapToItemDto<TDto>(itemResponse.Data);

        return result;
    }
    
    /// <summary>
    ///     Tags bookmarked items in an item collection.
    /// </summary>
    /// <param name="items"></param>
    protected async Task TagBookmarkedItems(List<TDto> items)
    {
        var bookmarkedItemIds = await ItemsRepository.GetBookmarkItemIds(TestUserId);
        
        if(bookmarkedItemIds.Count == 0) return;

        foreach (var item in items)
        {
            foreach (var bookmarkedItemId in bookmarkedItemIds)
            {
                if (bookmarkedItemId.Equals(item.Id))
                {
                    item.IsBookmarked = true;
                }
            }
        }
    }
}