using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.DependencyInjection;

namespace NewsPortal.Backend.Infrastructure;

public static class ConfigureServices
{
    /// <summary>
    ///     Configures Infrastructure Services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration manager reference.</param>
    public static void AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHackerNewsClient(cfg =>
        {
            cfg.BaseUrl = configuration["Apis:HackerNews:BaseUrl"]!;
            cfg.Version = int.Parse(configuration["Apis:HackerNews:Version"]!);
        });
    }
}