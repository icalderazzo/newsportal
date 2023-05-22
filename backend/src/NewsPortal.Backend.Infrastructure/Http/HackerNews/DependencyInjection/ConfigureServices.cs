using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Client;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.DependencyInjection;

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
        services.AddSingleton<IHackerNewsClient, HackerNewsClient>();
    }
}