namespace NewsPortal.Backend.Domain.Filters;

public class DbPaginationFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    
    public DbPaginationFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }
    
    public DbPaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 10 ? 10 : pageSize;
    }
}