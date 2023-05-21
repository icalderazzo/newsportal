using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Application.Workers;

namespace NewsPortal.Backend.Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        //  Worker Services
        services.AddHostedService<StoriesWorkerService>();
        
        //  Application Services
        services.AddScoped<IItemsCacheService, ItemsCacheService>();
        services.AddScoped<IItemService, ItemService>();

        //  AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}