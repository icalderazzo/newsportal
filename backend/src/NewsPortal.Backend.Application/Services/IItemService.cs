using NewsPortal.Backend.Contracts.Dtos;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;

namespace NewsPortal.Backend.Application.Services;

public interface IItemService
{
    Task<PagedResponse<List<ItemDto>>> GetNewestStories(PaginationFilter paginationFilter);
}