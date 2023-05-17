using AutoMapper;
using NewsPortal.Backend.Contracts.Dtos;

namespace NewsPortal.Backend.Application.Item;

internal class ItemMapper : Profile
{
    public ItemMapper()
    {
        CreateMap<Infrastructure.Http.HackerNews.Models.Contracts.Item, ItemDto>();
    }
}