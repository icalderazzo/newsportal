using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;
using NewsPortal.Backend.WebApi.Helpers;

namespace NewsPortal.Backend.WebApi.Controllers;

[ApiController]
[Route("stories")]
public class StoriesController : ControllerBase
{
    private readonly IStoriesService _storiesService;
    
    public StoriesController(IStoriesService storiesService)
    {
        _storiesService = storiesService;
    }

    /// <summary>
    ///     Gets the newest stories.
    /// </summary>
    /// <param name="paginationFilter"></param>
    /// <param name="searchString"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<List<StoryDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetStories([FromQuery] PaginationFilter paginationFilter, [FromQuery] string? searchString = null)
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

    /// <summary>
    ///     Gets the bookmarked stories of the user.
    /// </summary>
    /// <param name="paginationFilter"></param>
    /// <returns></returns>
    [HttpGet("bookmarks")]
    [ProducesResponseType(typeof(PagedResponse<List<StoryDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetBookmarkedStories([FromQuery] PaginationFilter paginationFilter)
    {
        try
        {
            var bookmarks = await _storiesService.GetBookmarks(paginationFilter);
            return Ok(bookmarks);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     Bookmarks story for the logged user
    /// </summary>
    /// <param name="storyId"></param>
    /// <returns></returns>
    [HttpPost("{storyId:int}/bookmark")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> BookmarkStory(int storyId)
    {
        try
        {
            await _storiesService.BookmarkItem(storyId);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}