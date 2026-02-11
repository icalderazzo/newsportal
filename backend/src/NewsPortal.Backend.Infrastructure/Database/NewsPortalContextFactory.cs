using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NewsPortal.Backend.Infrastructure.Database;

public class NewsPortalContextFactory : IDbContextFactory<NewsPortalContext>
{
    private readonly IConfiguration _configuration;
    
    public NewsPortalContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public NewsPortalContext CreateDbContext()
    {
        var connString = _configuration.GetConnectionString("Default");
        var optionsBuilder = new DbContextOptionsBuilder<NewsPortalContext>();
        optionsBuilder.UseSqlServer(connString);
        
        return new NewsPortalContext(optionsBuilder.Options);
    }
}