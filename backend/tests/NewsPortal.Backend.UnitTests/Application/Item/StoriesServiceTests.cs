using System.Net;
using Moq;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.Domain.Filters;
using NewsPortal.Backend.Domain.Models.Items;
using NewsPortal.Backend.Domain.Repositories;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;
using NewsPortal.Backend.UnitTests.Application.Item.MockData;

namespace NewsPortal.Backend.UnitTests.Application.Item;

[TestFixture]
public class StoriesServiceTests : BaseItemServiceTestFixture
{
    [SetUp]
    public void SetUp()
    {
        HackerNewsClient = new Mock<IHackerNewsClient>();
        ItemsRepository = new Mock<IItemsRepository>();
        ItemsCacheService = new Mock<IItemsCacheService>();
        _storiesService = new StoriesService(
            HackerNewsClient.Object,
            ItemsRepository.Object,
            ItemsCacheService.Object,
            ItemMapper,
            FilterMapper);
    }

    private StoriesService _storiesService;

    [Test]
    public async Task GetNewestStories_NoPagination_Success()
    {
        //  Arrange
        var itemsCount = HackerNewsData.Items.Count;
        var expected = new PagedResponse<List<StoryDto>>
        {
            Data = StoriesData.Stories,
            TotalRecords = itemsCount
        };

        //  Client mocking
        HackerNewsClient.Setup(c =>
                c.GetNewStories())
            .ReturnsAsync(
                new HackerNewsClientResponse<List<int>>
                {
                    Code = HttpStatusCode.OK,
                    Success = true,
                    Data = HackerNewsData.NewestStories
                });
        //  Cache service mocking
        ItemsCacheService.Setup(i =>
                i.GetOrCreateItems(It.IsAny<IEnumerable<int>>(), It.IsAny<Func<int, Task<StoryDto?>>>()))
            .ReturnsAsync(StoriesData.Stories);

        //  Act
        var result = await _storiesService.GetNewestStories();

        // Assert
        Assert.That(expected.Data, Is.EquivalentTo(result.Data));
        Assert.That(expected.TotalRecords, Is.EqualTo(result.TotalRecords));
        //  Check if get new stories has been called once
        HackerNewsClient.Verify(c => c.GetNewStories(), Times.Once);
        //  Check if all id's have been passed to GetOrCreateItemsMethod and it has been executed once
        ItemsCacheService.Verify(i =>
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == itemsCount),
                    It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }

    [Test]
    public async Task GetNewestStories_Pagination_Success()
    {
        //  Arrange
        var paginationFilter = new PaginationFilter { PageSize = 2, PageNumber = 1 };
        var expected = new PagedResponse<List<StoryDto>>
        {
            Data = StoriesData.Stories.Take(paginationFilter.PageSize).ToList(),
            TotalRecords = StoriesData.Stories.Count
        };

        //  Client mocking
        HackerNewsClient.Setup(c =>
                c.GetNewStories())
            .ReturnsAsync(
                new HackerNewsClientResponse<List<int>>
                {
                    Code = HttpStatusCode.OK,
                    Success = true,
                    Data = HackerNewsData.NewestStories
                });
        //  Cache service mocking
        ItemsCacheService.Setup(i =>
                i.GetOrCreateItems(It.IsAny<IEnumerable<int>>(), It.IsAny<Func<int, Task<StoryDto?>>>()))
            .ReturnsAsync(StoriesData.Stories.Take(paginationFilter.PageSize).ToList);

        //  Act
        var result = await _storiesService.GetNewestStories(paginationFilter);

        // Assert
        Assert.That(expected.Data, Is.EquivalentTo(result.Data));
        Assert.That(expected.TotalRecords, Is.EqualTo(result.TotalRecords));
        //  Check if get new stories has been called once
        HackerNewsClient.Verify(c => c.GetNewStories(), Times.Once);
        //  Check if only the ids matching the page size have been passed to GetOrCreateItemsMethod and it has been executed once
        ItemsCacheService.Verify(i =>
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == paginationFilter.PageSize),
                    It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }

    [Test]
    public async Task Search_ResultsFound_Success()
    {
        //  Arrange
        var defaultPaginationFilter = new PaginationFilter();
        var filteredList = StoriesData.Stories
            .Where(x => x.Title.Contains('1', StringComparison.OrdinalIgnoreCase))
            .ToList();

        var expected = new PagedResponse<List<StoryDto>>
        {
            Data = filteredList,
            TotalRecords = filteredList.Count
        };

        //  Client mocking
        HackerNewsClient.Setup(c =>
                c.GetNewStories())
            .ReturnsAsync(
                new HackerNewsClientResponse<List<int>>
                {
                    Code = HttpStatusCode.OK,
                    Success = true,
                    Data = HackerNewsData.NewestStories
                });
        //  Cache service mocking
        ItemsCacheService.Setup(i =>
                i.GetOrCreateItems(It.IsAny<IEnumerable<int>>(), It.IsAny<Func<int, Task<StoryDto?>>>()))
            .ReturnsAsync(StoriesData.Stories);

        //  Act
        var result = await _storiesService.Search("1", defaultPaginationFilter);

        // Assert
        Assert.That(expected.Data, Is.EquivalentTo(result.Data));
        Assert.That(expected.TotalRecords, Is.EqualTo(result.TotalRecords));
        //  Check if get new stories has been called once
        HackerNewsClient.Verify(c => c.GetNewStories(), Times.Once);
        //  Check if all id's have been passed to GetOrCreateItemsMethod and it has been executed once
        ItemsCacheService.Verify(i =>
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == StoriesData.Stories.Count),
                    It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }

    [Test]
    public async Task Search_NoResultsFound_Success()
    {
        //  Arrange
        const string searchString = "no coincidences";
        var defaultPaginationFilter = new PaginationFilter();

        var expected = new PagedResponse<List<StoryDto>>
        {
            Data = new List<StoryDto>(),
            TotalRecords = 0
        };

        //  Client mocking
        HackerNewsClient.Setup(c =>
                c.GetNewStories())
            .ReturnsAsync(
                new HackerNewsClientResponse<List<int>>
                {
                    Code = HttpStatusCode.OK,
                    Success = true,
                    Data = HackerNewsData.NewestStories
                });
        //  Cache service mocking
        ItemsCacheService.Setup(i =>
                i.GetOrCreateItems(It.IsAny<IEnumerable<int>>(), It.IsAny<Func<int, Task<StoryDto?>>>()))
            .ReturnsAsync(StoriesData.Stories);

        //  Act
        var result = await _storiesService.Search(searchString, defaultPaginationFilter);

        // Assert
        Assert.That(expected.Data, Is.EquivalentTo(result.Data));
        Assert.That(expected.TotalRecords, Is.EqualTo(result.TotalRecords));
        //  Check if get new stories has been called once
        HackerNewsClient.Verify(c => c.GetNewStories(), Times.Once);
        //  Check if all id's have been passed to GetOrCreateItemsMethod and it has been executed once
        ItemsCacheService.Verify(i =>
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == StoriesData.Stories.Count),
                    It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }

    #region BookmarkItem Tests

    [Test]
    public async Task BookmarkItem_ItemExists_Success()
    {
        //  Arrange
        const int itemId = 1;

        //  Mock that item exists in database
        ItemsRepository.Setup(r => r.Exists(itemId))
            .ReturnsAsync(true);

        //  Mock bookmark item
        ItemsRepository.Setup(r => r.BookmarkItem(It.IsAny<UserItem>()))
            .ReturnsAsync((UserItem ui) => ui);

        //  Act
        await _storiesService.BookmarkItem(itemId);

        //  Assert
        //  Verify Exists was called once with the correct itemId
        ItemsRepository.Verify(r => r.Exists(itemId), Times.Once);

        //  Verify BookmarkItem was called once with the correct userId and itemId
        ItemsRepository.Verify(r =>
                r.BookmarkItem(It.Is<UserItem>(ui =>
                    ui.ItemId == itemId && ui.UserId == 1)),
            Times.Once);

        //  Verify GetItemById was NOT called since item exists
        HackerNewsClient.Verify(c => c.GetItemById(It.IsAny<int>()), Times.Never);

        //  Verify Save was NOT called since item exists
        ItemsRepository.Verify(r => r.Save(It.IsAny<Story>()), Times.Never);
    }

    [Test]
    public async Task BookmarkItem_ItemNotExists_FetchAndSave_Success()
    {
        //  Arrange
        const int itemId = 5;
        var itemDto = new StoryDto
        {
            Id = itemId,
            Title = "NewStory",
            Type = "story",
            Url = "https://newstory.com"
        };

        //  Mock that item does NOT exist in database
        ItemsRepository.Setup(r => r.Exists(itemId))
            .ReturnsAsync(false);

        //  Mock HackerNews client to return item
        HackerNewsClient.Setup(c => c.GetItemById(itemId))
            .ReturnsAsync(
                new HackerNewsClientResponse<Infrastructure.Http.HackerNews.Models.Contracts.Item>
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = new Infrastructure.Http.HackerNews.Models.Contracts.Item
                    {
                        Id = itemId,
                        Title = "NewStory",
                        Type = "story",
                        Url = "https://newstory.com"
                    }
                });

        //  Mock Save
        ItemsRepository.Setup(r => r.Save(It.IsAny<Story>()))
            .ReturnsAsync((Story s) => s);

        //  Mock bookmark item
        ItemsRepository.Setup(r => r.BookmarkItem(It.IsAny<UserItem>()))
            .ReturnsAsync((UserItem ui) => ui);

        //  Act
        await _storiesService.BookmarkItem(itemId);

        //  Assert
        //  Verify Exists was called once with the correct itemId
        ItemsRepository.Verify(r => r.Exists(itemId), Times.Once);

        //  Verify GetItemById was called once with the correct itemId
        HackerNewsClient.Verify(c => c.GetItemById(itemId), Times.Once);

        //  Verify Save was called once with a Story object
        ItemsRepository.Verify(r =>
                r.Save(It.Is<Story>(s =>
                    s.Id == itemId && s.Title == "NewStory")),
            Times.Once);

        //  Verify BookmarkItem was called once with the correct userId and itemId
        ItemsRepository.Verify(r =>
                r.BookmarkItem(It.Is<UserItem>(ui =>
                    ui.ItemId == itemId && ui.UserId == 1)),
            Times.Once);
    }

    [Test]
    public async Task BookmarkItem_ItemNotExists_ApiFails_ThrowsException()
    {
        //  Arrange
        const int itemId = 6;

        //  Mock that item does NOT exist in database
        ItemsRepository.Setup(r => r.Exists(itemId))
            .ReturnsAsync(false);

        //  Mock HackerNews client to return failure
        HackerNewsClient.Setup(c => c.GetItemById(itemId))
            .ReturnsAsync(
                new HackerNewsClientResponse<Infrastructure.Http.HackerNews.Models.Contracts.Item>
                {
                    Code = HttpStatusCode.InternalServerError,
                    Success = false,
                    Data = null
                });

        //  Act & Assert - Expect NullReferenceException when trying to map null item
        Assert.That(async () => await _storiesService.BookmarkItem(itemId), 
            Throws.TypeOf<NullReferenceException>());

        //  Verify Exists was called
        ItemsRepository.Verify(r => r.Exists(itemId), Times.Once);

        //  Verify GetItemById was called
        HackerNewsClient.Verify(c => c.GetItemById(itemId), Times.Once);

        //  Verify BookmarkItem was NOT called since item fetch failed
        ItemsRepository.Verify(r =>
                r.BookmarkItem(It.IsAny<UserItem>()),
            Times.Never);
    }

    #endregion

    #region GetBookmarks Tests

    [Test]
    public async Task GetBookmarks_NoPagination_Success()
    {
        //  Arrange
        const int userId = 1;
        var bookmarkedItems = UserItemsData.BookmarkedItems;

        //  Mock GetBookmarkItems
        ItemsRepository.Setup(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()))
            .ReturnsAsync(bookmarkedItems);

        //  Mock CountBookmarkItems
        ItemsRepository.Setup(r => r.CountBookmarkItems(userId))
            .ReturnsAsync(bookmarkedItems.Count);

        //  Act
        var result = await _storiesService.GetBookmarks();

        //  Assert
        Assert.That(result.Data.Count, Is.EqualTo(bookmarkedItems.Count));
        Assert.That(result.TotalRecords, Is.EqualTo(bookmarkedItems.Count));
        //  Verify data contains correct items by checking IDs
        Assert.That(result.Data.Select(d => d.Id), Is.EquivalentTo(bookmarkedItems.Select(ui => ui.Item.Id)));

        //  Verify GetBookmarkItems was called once with userId and null filter
        ItemsRepository.Verify(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()),
            Times.Once);

        //  Verify CountBookmarkItems was called once with userId
        ItemsRepository.Verify(r => r.CountBookmarkItems(userId), Times.Once);
    }

    [Test]
    public async Task GetBookmarks_WithPagination_Success()
    {
        //  Arrange
        const int userId = 1;
        var paginationFilter = new PaginationFilter { PageSize = 2, PageNumber = 1 };
        var allBookmarkedItems = UserItemsData.BookmarkedItems;
        var pagedItems = allBookmarkedItems.Take(paginationFilter.PageSize).ToList();

        //  Mock GetBookmarkItems
        ItemsRepository.Setup(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()))
            .ReturnsAsync(pagedItems);

        //  Mock CountBookmarkItems
        ItemsRepository.Setup(r => r.CountBookmarkItems(userId))
            .ReturnsAsync(allBookmarkedItems.Count);

        //  Act
        var result = await _storiesService.GetBookmarks(paginationFilter);

        //  Assert
        Assert.That(result.Data.Count, Is.EqualTo(paginationFilter.PageSize));
        Assert.That(result.TotalRecords, Is.EqualTo(allBookmarkedItems.Count));
        Assert.That(result.Data.Select(d => d.Id), Is.EquivalentTo(pagedItems.Select(ui => ui.Item.Id)));

        //  Verify GetBookmarkItems was called once with userId and a filter
        ItemsRepository.Verify(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()),
            Times.Once);

        //  Verify CountBookmarkItems was called once with userId
        ItemsRepository.Verify(r => r.CountBookmarkItems(userId), Times.Once);
    }

    [Test]
    public async Task GetBookmarks_NoResults_Success()
    {
        //  Arrange
        const int userId = 1;
        var emptyBookmarks = new List<UserItem>();

        //  Mock GetBookmarkItems to return empty list
        ItemsRepository.Setup(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()))
            .ReturnsAsync(emptyBookmarks);

        //  Mock CountBookmarkItems to return 0
        ItemsRepository.Setup(r => r.CountBookmarkItems(userId))
            .ReturnsAsync(0);

        //  Act
        var result = await _storiesService.GetBookmarks();

        //  Assert
        Assert.That(result.Data.Count, Is.Zero);
        Assert.That(result.TotalRecords, Is.Zero);

        //  Verify GetBookmarkItems was called once
        ItemsRepository.Verify(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()),
            Times.Once);

        //  Verify CountBookmarkItems was called once
        ItemsRepository.Verify(r => r.CountBookmarkItems(userId), Times.Once);
    }

    [Test]
    public async Task GetBookmarks_WithPaginationPage2_Success()
    {
        //  Arrange
        const int userId = 1;
        var paginationFilter = new PaginationFilter { PageSize = 1, PageNumber = 2 };
        var allBookmarkedItems = UserItemsData.BookmarkedItems;
        var pagedItems = allBookmarkedItems.Skip(1).Take(1).ToList();

        //  Mock GetBookmarkItems
        ItemsRepository.Setup(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()))
            .ReturnsAsync(pagedItems);

        //  Mock CountBookmarkItems
        ItemsRepository.Setup(r => r.CountBookmarkItems(userId))
            .ReturnsAsync(allBookmarkedItems.Count);

        //  Act
        var result = await _storiesService.GetBookmarks(paginationFilter);

        //  Assert
        Assert.That(result.Data.Count, Is.EqualTo(1));
        Assert.That(result.TotalRecords, Is.EqualTo(allBookmarkedItems.Count));
        Assert.That(result.Data.Select(d => d.Id), Is.EquivalentTo(pagedItems.Select(ui => ui.Item.Id)));

        //  Verify repository calls
        ItemsRepository.Verify(r =>
                r.GetBookmarkItems(userId, It.IsAny<DbPaginationFilter>()),
            Times.Once);

        ItemsRepository.Verify(r => r.CountBookmarkItems(userId), Times.Once);
    }

    #endregion
}