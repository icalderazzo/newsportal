using NewsPortal.Backend.Contracts.Dtos;

namespace NewsPortal.Backend.Application.Services;

public interface IItemService
{
    Task<List<ItemDto>> GetNewestStories();
}