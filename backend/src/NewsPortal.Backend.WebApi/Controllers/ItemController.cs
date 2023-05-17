using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;

namespace NewsPortal.Backend.WebApi.Controllers;

[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    
    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    /// <summary>
    ///     Gets the newest stories.
    /// </summary>
    /// <param name="paginationFilter"></param>
    /// <returns></returns>
    [HttpGet("stories")]
    [ProducesResponseType(typeof(PagedResponse<List<ItemDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetStories([FromQuery] PaginationFilter paginationFilter)
    {
        try
        {
            return Ok(await _itemService.GetNewestStories(paginationFilter));
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}