using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;

namespace NewsPortal.Backend.WebApi.Helpers;

public static class PaginationHelper
{
    /// <summary>
    ///     Fills paged response TotalPages, PreviousPage URI and NextPage URI.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="paginationFilter"></param>
    /// <param name="baseUri"></param>
    /// <param name="searchString"></param>
    /// <typeparam name="T"></typeparam>
    public static void FillPagedResponseData<T>(this PagedResponse<List<T>> response,
        PaginationFilter paginationFilter, Uri baseUri, string? searchString = null)
    {
        //  Calculate total pages
        response.TotalPages = Convert.ToInt32(response.TotalRecords / paginationFilter.PageSize);
        
        //  Build next page uri
        response.NextPage = paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < response.TotalPages 
            ? BuildPageUri(paginationFilter.PageNumber + 1, paginationFilter.PageSize, baseUri, searchString)
            : null;
        
        //  Build previous page uri
        response.PreviousPage = paginationFilter.PageNumber > 1 && paginationFilter.PageNumber <= response.TotalPages
            ? BuildPageUri(paginationFilter.PageNumber - 1, paginationFilter.PageSize, baseUri, searchString)
            : null;
    }

    private static Uri BuildPageUri(int pageNumber, int pageSize, Uri baseUri, string? searchString = null)
    {
        var searchStringParam = string.IsNullOrEmpty(searchString) ? string.Empty : $"{searchString}&"; 
        var builder = new UriBuilder(baseUri)
        {
            Query = $"{searchStringParam}pageNumber={pageNumber}&pageSize={pageSize}"
        };
        return builder.Uri;
    }
}