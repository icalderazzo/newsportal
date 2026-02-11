using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Backend.Application.BackgroundServices;
using NewsPortal.Backend.Application.Item;
using NewsPortal.Backend.Application.Mappers;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Domain.Models.Items;
using ItemMapper = NewsPortal.Backend.Application.Mappers.ItemMapper;

namespace NewsPortal.Backend.Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        //  Background Hosted Services
        services.AddHostedService<NewestStoriesBackgroundService>();
        services.AddHostedService<UpdateStoriesBackgroundService>();

        services.AddScoped<IItemsCacheService, ItemsCacheService>();
        services.AddScoped<IItemService<Story, StoryDto>, StoriesService>();
        services.AddScoped<IStoriesService, StoriesService>();

        //  Mapperly
        services.AddSingleton<ItemMapper>();
        services.AddSingleton<FilterMapper>();
    }
}