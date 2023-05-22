using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Client;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.DependencyInjection;

namespace NewsPortal.Backend.Infrastructure;

public static class ConfigureServices
{
    /// <summary>
    ///     Configures Infrastructure Services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="hackerNewsClientConfig">HackerNewsClient configuration.</param>
    public static void AddInfrastructureServices(this IServiceCollection services, 
        Action<HackerNewsOptions> hackerNewsClientConfig)
    {
        services.AddHackerNewsClient(hackerNewsClientConfig);
    }
}