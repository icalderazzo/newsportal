using System.Net;
using AutoMapper;
using Moq;
using NewsPortal.Backend.Application.Item.Story;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.Contracts.Filters;
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
        ItemsCacheService = new Mock<IItemsCacheService>();
        _storiesService = new StoriesService(HackerNewsClient.Object, ItemsCacheService.Object, Mapper);
    }

    [Test]
    public async Task GetNewestStories_NoPagination_Success()
    {
        //  Arrange
        var itemsCount = HackerNewsData.Items.Count;
        var expected = new ValueTuple<List<StoryDto>, int>()
        {
            Item1 = StoriesData.Stories,
            Item2 = itemsCount
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
        Assert.That(expected.Item1, Is.EquivalentTo(result.Item1));
        Assert.That(expected.Item2, Is.EqualTo(result.Item2));
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
        var expected = new ValueTuple<List<StoryDto>, int>()
        {
            Item1 = StoriesData.Stories.Take(paginationFilter.PageSize).ToList(),
            Item2 = StoriesData.Stories.Count
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
        Assert.That(expected.Item1, Is.EquivalentTo(result.Item1));
        Assert.That(expected.Item2, Is.EqualTo(result.Item2));
        //  Check if get new stories has been called once
        HackerNewsClient.Verify(c => c.GetNewStories(), Times.Once);
        //  Check if only the ids matching the page size have been passed to GetOrCreateItemsMethod and it has been executed once
        ItemsCacheService.Verify(i => 
                i.GetOrCreateItems(It.Is<List<int>>(l => l.Count == paginationFilter.PageSize), It.IsAny<Func<int, Task<StoryDto?>>>()),
            Times.Once);
    }
}