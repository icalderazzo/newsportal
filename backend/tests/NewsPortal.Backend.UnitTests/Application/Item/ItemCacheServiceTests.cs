using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.UnitTests.Application.Item.MockData;

namespace NewsPortal.Backend.UnitTests.Application.Item;

[TestFixture]
public class ItemCacheServiceTests
{
    private readonly ServiceCollection _services = new();
    private IServiceProvider _serviceProvider;
    private IMemoryCache _memoryCache;
    private ItemsCacheService _itemsCacheService;
    private Func<int, Task<StoryDto?>> _createStoryFunc;
    
    [SetUp]
    public void Setup()
    {
        _services.AddMemoryCache();
        _serviceProvider = _services.BuildServiceProvider();
        
        _memoryCache = _serviceProvider.GetService<IMemoryCache>()!;
        _itemsCacheService = new ItemsCacheService(_memoryCache);
        
        _createStoryFunc = async i =>
        {
            await Task.Delay(10);
            return new StoryDto
            {
                Id = i,
                Title = $"Story{i}",
                Url = $"https://story{i}.com",
                Type = "story"
            };
        };
    }

    [Test]
    public async Task GetOrCreateItems_Stories_Success()
    {
        //  Arrange
        var newStoryIds = new List<int> {6, 5, 4, 3, 2, 1};
        var expectedItemCount = newStoryIds.Count;

        //  Load cache with the 4 initial stories
        foreach (var story in StoriesData.Stories)
            _memoryCache.Set(story.Id, story);
        
        //  Act
        var result = 
            (await _itemsCacheService.GetOrCreateItems(newStoryIds, _createStoryFunc))
            .ToList();
        
        //  Assert
        //  Check if stories 6 and 5 were added and service returned the 4 initial stories and the 2 new ones.
        Assert.That(result, Has.Count.EqualTo(expectedItemCount));
    }
    
    [Test]
    public async Task GetOrCreateItems_Stories_FailedToCreateNewItems_ReturnsCachedStories()
    {
        //  Arrange
        var newStoryIds = new List<int> {6, 5, 4, 3, 2, 1};
        var expectedItemCount = StoriesData.Stories.Count;

        //  Load cache with the 4 initial stories
        foreach (var story in StoriesData.Stories)
            _memoryCache.Set(story.Id, story);
        
        //  Act
        var result = 
            (await _itemsCacheService.GetOrCreateItems<StoryDto>(newStoryIds, async(i) =>
            {
                await Task.Delay(10);
                return null;
            }))
            .ToList();
        
        //  Assert
        //  Check if stories 6 and 5 were not created and service returned result is the initial 4 stories
        Assert.That(result, Has.Count.EqualTo(expectedItemCount));
    }
    
    [TearDown]
    public void TearDown()
    {
        (_serviceProvider as IDisposable)?.Dispose();
    }
}