using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Infrastructure.Http.HackerNews;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Client;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Configuration;

namespace NewsPortal.Backend.Infrastructure;

public static class ConfigureServices
{
    /// <summary>
    ///     Configures the HackerNews Http Client.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="config">Client configuration.</param>
    public static void AddHackerNewsClient(this IServiceCollection services, Action<HackerNewsOptions> config)
    {
        services.Configure(config);
        services.AddScoped<IHackerNewsClient, HackerNewsClient>();
    }
}