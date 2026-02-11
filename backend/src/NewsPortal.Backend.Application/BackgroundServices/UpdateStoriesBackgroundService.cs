using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Contracts.Dtos.Item;
using NewsPortal.Backend.Domain.Models.Items;

namespace NewsPortal.Backend.Application.BackgroundServices;

public class UpdateStoriesBackgroundService : IHostedService, IDisposable
{
    private readonly ILogger<UpdateStoriesBackgroundService> _logger;
    private readonly TimeSpan _period = TimeSpan.FromMinutes(15);
    private readonly IServiceProvider _serviceProvider;
    private int _executionCount;
    private Timer? _timer;

    public UpdateStoriesBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<UpdateStoriesBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(_ => { Task.Run(async () => { await DoWork(); }, cancellationToken); },
            null,
            TimeSpan.Zero,
            _period);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(UpdateStoriesBackgroundService)} is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Updates stories.
    /// </summary>
    private async Task DoWork()
    {
        _logger.LogInformation($"{nameof(UpdateStoriesBackgroundService)} is working.");

        using var scope = _serviceProvider.CreateScope();
        var itemService = scope.ServiceProvider.GetRequiredService<IItemService<Story, StoryDto>>();
        await itemService.UpdateItems();

        _executionCount++;
        _logger.LogInformation($"{nameof(UpdateStoriesBackgroundService)} executed. Count: {_executionCount}");
    }
}