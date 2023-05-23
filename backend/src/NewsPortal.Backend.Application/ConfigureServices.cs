using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Application.BackgroundServices;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Application.Item.Story;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item.Story;

namespace NewsPortal.Backend.Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        //  Background Hosted Services
        services.AddHostedService<NewestStoriesBackgroundService>();
        services.AddHostedService<UpdateStoriesBackgroundService>();
        
        services.AddScoped<IItemsCacheService, ItemsCacheService>();
        services.AddScoped<IItemService<StoryDto>, StoriesService>();
        services.AddScoped<IStoriesService, StoriesService>();

        //  AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}