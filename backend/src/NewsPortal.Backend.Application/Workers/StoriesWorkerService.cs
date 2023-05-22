using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsPortal.Backend.Application.Services;

namespace NewsPortal.Backend.Application.Workers;

public class StoriesWorkerService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StoriesWorkerService> _logger;
    private bool _workCompleted;
    
    public StoriesWorkerService(
        IServiceProvider serviceProvider, 
        ILogger<StoriesWorkerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_workCompleted)
            await Task.CompletedTask;

        try
        {
            await DoWork(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError($"{nameof(StoriesWorkerService)} has thrown an exception: {e.Message}");

        }
        finally
        {
            _workCompleted = true;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(StoriesWorkerService)} stopped.");
        await Task.CompletedTask;
    }
    
    private async Task DoWork(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(StoriesWorkerService)} is working.");

        using var scope = _serviceProvider.CreateScope();
        var itemService = scope.ServiceProvider.GetRequiredService<IItemService>();
        await itemService.GetNewestStories();
    }
}