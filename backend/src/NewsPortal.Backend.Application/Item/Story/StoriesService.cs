using System.Runtime.CompilerServices;
using AutoMapper;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;

[assembly: InternalsVisibleTo("NewsPortal.Backend.UnitTests")]
namespace NewsPortal.Backend.Application.Item.Story;

internal class StoriesService : BaseItemService<StoryDto>, IStoriesService 
{
    public StoriesService(
        IHackerNewsClient hackerNewsClient,  
        IItemsCacheService itemsCacheService,
        IMapper mapper) : base(hackerNewsClient, itemsCacheService, mapper)
    {
    }

    public async Task<(List<StoryDto>, int)> GetNewestStories(PaginationFilter? paginationFilter = null)
    {
        //  Get new story ids
        var newStoriesResponse = await HackerNewsClient.GetNewStories();
        var newStories = newStoriesResponse.Data!;
        
        //  Apply pagination if filter has been passed
        if (paginationFilter is not null)
            newStories = newStories
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();
        
        //  Get stories from cache service
        var items = await ItemsCacheService.GetOrCreateItems(newStories, GetItemById);

        //  Order stories list by newest
        items = items.OrderByDescending(x => x.Id).ToList();
        
        return new ValueTuple<List<StoryDto>, int>(items, newStoriesResponse.Data!.Count);
    }

    public async Task<(List<StoryDto>, int)> Search(string searchString, PaginationFilter paginationFilter)
    {
        //  Get new story ids
        var newStoriesResponse = await HackerNewsClient.GetNewStories();

        //  Get items from cache service
        var items = await ItemsCacheService.GetOrCreateItems(newStoriesResponse.Data!, GetItemById);
        
        //  Filter items where title contains the search string
        //  Paginate the result
        items = items
            .Where(i => i.Title.ToLower().Trim().Contains(searchString.Trim().ToLower()))
            .OrderByDescending(i => i.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToList();
        
        return new ValueTuple<List<StoryDto>, int>(items, items.Count);
    }
}