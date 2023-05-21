using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.WebApi.Helpers;

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
    /// <param name="searchString"></param>
    /// <returns></returns>
    [HttpGet("stories")]
    [ProducesResponseType(typeof(PagedResponse<List<ItemDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetStories(
        [FromQuery] PaginationFilter paginationFilter,
        [FromQuery] string? searchString = null)
    {
        try
        {
            ValueTuple<List<ItemDto>, int> storiesResult;
            if (string.IsNullOrEmpty(searchString))
            {
                storiesResult = await _itemService.GetNewestStories(paginationFilter);
            }
            else
            {
                storiesResult = await _itemService.SearchNews(searchString, paginationFilter);
            }
            
            var response = PaginationHelper.CreatePagedResponse(
                storiesResult.Item1, 
                paginationFilter, 
                storiesResult.Item2, 
                UriHelper.GetRequestBaseUri(Request),
                searchString);
            
            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}