using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Filters;

namespace NewsPortal.Backend.WebApi.Controllers;

[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    
    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet("stories")]
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