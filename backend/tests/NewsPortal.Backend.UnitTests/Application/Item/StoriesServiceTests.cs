using System.Net;
using Moq;
using NewsPortal.Backend.Application.Item.Story;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.Domain.Repositories;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;
using NewsPortal.Backend.UnitTests.Application.Item.MockData;

namespace NewsPortal.Backend.UnitTests.Application.Item;

[TestFixture]
public class StoriesServiceTests : BaseItemServiceTestFixture
{
    private StoriesService _storiesService;
    
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
            Mapper);
    }

    #region GetNewestStoriesTests
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
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == itemsCount), It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }

    [Test]
    public async Task GetNewestStories_Pagination_Success()
    {
        //  Arrange
        var paginationFilter = new PaginationFilter {PageSize = 2, PageNumber = 1};
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
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == paginationFilter.PageSize), It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }
    #endregion

    #region SearchTests
    [Test]
    public async Task Search_ResultsFound_Success()
    {
        //  Arrange
        var defaultPaginationFilter = new PaginationFilter();
        var filteredList = StoriesData.Stories
            .Where(x => x.Title.Contains('1', StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        var expected = new PagedResponse<List<StoryDto>>()
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
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == StoriesData.Stories.Count), It.IsAny<Func<int, Task<StoryDto?>>>()),
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
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == StoriesData.Stories.Count), It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }
    #endregion
}