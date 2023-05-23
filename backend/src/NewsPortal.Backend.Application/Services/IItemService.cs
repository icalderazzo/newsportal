using NewsPortal.Backend.Contracts.Dtos.Item;

namespace NewsPortal.Backend.Application.Services;

public interface IItemService<T> where T : ItemDto
{
    Task UpdateItems();
}