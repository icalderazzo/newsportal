using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsPortal.Backend.Application.Services;

namespace NewsPortal.Backend.Application.Workers;

public class StoriesWorkerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StoriesWorkerService> _logger;
    
    public StoriesWorkerService(
        IServiceProvider serviceProvider, 
        ILogger<StoriesWorkerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await DoWork(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError($"{nameof(StoriesWorkerService)} has thrown an exception: {e.Message}");
        }
    }
    
    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(StoriesWorkerService)} is working.");

        using var scope = _serviceProvider.CreateScope();
        var itemService = scope.ServiceProvider.GetRequiredService<IItemService>();
        await itemService.GetNewestStories();
    }
}