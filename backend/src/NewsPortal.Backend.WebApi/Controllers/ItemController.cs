using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.WebApi.Helpers;

namespace NewsPortal.Backend.WebApi.Controllers;

[ApiController]
public class ItemController : ControllerBase
{
    private readonly IStoriesService _storiesService;
    
    public ItemController(IStoriesService storiesService)
    {
        _storiesService = storiesService;
    }

    /// <summary>
    ///     Gets the newest stories.
    /// </summary>
    /// <param name="paginationFilter"></param>
    /// <param name="searchString"></param>
    /// <returns></returns>
    [HttpGet("stories")]
    [ProducesResponseType(typeof(PagedResponse<List<StoryDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetStories(
        [FromQuery] PaginationFilter paginationFilter,
        [FromQuery] string? searchString = null)
    {
        try
        {
            PagedResponse<List<StoryDto>>? response;
            
            if (string.IsNullOrEmpty(searchString))
            {
                response = await _storiesService.GetNewestStories(paginationFilter);
            }
            else
            {
                response = await _storiesService.Search(searchString, paginationFilter);
            }
            
            response.FillPagedResponseData(paginationFilter, Request.GetRequestBaseUri(), searchString);
            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}