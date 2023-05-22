using AutoMapper;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;

namespace NewsPortal.Backend.Application.Item;

internal class ItemMapper : Profile
{
    public ItemMapper()
    {
        CreateMap<Infrastructure.Http.HackerNews.Models.Contracts.Item, StoryDto>();
    }
}