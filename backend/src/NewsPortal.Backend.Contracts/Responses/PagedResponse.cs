namespace NewsPortal.Backend.Contracts.Responses;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri? PreviousPage { get; set; }
    public Uri? NextPage { get; set; }

    public PagedResponse()
    {
        
    }
    
    public PagedResponse(T data, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Data = data;
    }
}