namespace NewsPortal.Backend.Contracts.Responses;

public class PagedResponse<T> : Response<T>
{
    public PagedResponse()
    {
    }

    public PagedResponse(T data, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Data = data;
    }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri? PreviousPage { get; set; }
    public Uri? NextPage { get; set; }
}