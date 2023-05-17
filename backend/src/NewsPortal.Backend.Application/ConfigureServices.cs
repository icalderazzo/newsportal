using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Application.Services;

namespace NewsPortal.Backend.Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        //  Application Services
        services.AddScoped<IItemService, ItemService>();

        //  AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}