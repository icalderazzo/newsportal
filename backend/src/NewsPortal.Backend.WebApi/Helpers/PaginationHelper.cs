﻿using NewsPortal.Backend.Contracts.Filters;
using NewsPortal.Backend.Contracts.Responses;

namespace NewsPortal.Backend.WebApi.Helpers;

public static class PaginationHelper
{
    /// <summary>
    ///     Builds a paged response.
    /// </summary>
    /// <param name="data">Already paged data.</param>
    /// <param name="paginationFilter">The pagination filter used to page data.</param>
    /// <param name="totalRecords">Total record count.</param>
    /// <param name="baseUri">Base URI of the resource.</param>
    /// <typeparam name="T">Type of the object of the response.</typeparam>
    /// <returns></returns>
    public static PagedResponse<List<T>> CreatePagedResponse<T>(
        List<T> data, PaginationFilter paginationFilter, int totalRecords, Uri baseUri)
    {
        var response = new PagedResponse<List<T>>(data, paginationFilter.PageNumber, paginationFilter.PageSize);
        var totalPages = Convert.ToInt32(totalRecords / paginationFilter.PageSize);

        response.NextPage = paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < totalPages 
            ? BuildPageUri(paginationFilter.PageNumber + 1, paginationFilter.PageSize, baseUri)
            : null;
        response.PreviousPage = paginationFilter.PageNumber > 1 && paginationFilter.PageNumber <= totalPages 
            ? BuildPageUri(paginationFilter.PageNumber - 1, paginationFilter.PageSize, baseUri)
            : null;
        
        response.TotalPages = totalPages;
        response.TotalRecords = totalRecords;

        return response;
    }

    private static Uri BuildPageUri(int pageNumber, int pageSize, Uri baseUri)
    {
        var builder = new UriBuilder(baseUri)
        {
            Query = $"pageNumber={pageNumber}&pageSize={pageSize}"
        };
        return builder.Uri;
    }
}