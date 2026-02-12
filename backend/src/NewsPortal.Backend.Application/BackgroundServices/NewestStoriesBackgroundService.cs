using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsPortal.Backend.Application.Services;

namespace NewsPortal.Backend.Application.BackgroundServices;

public class NewestStoriesBackgroundService : IHostedService
{
    private readonly ILogger<NewestStoriesBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private bool _workCompleted;

    public NewestStoriesBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<NewestStoriesBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_workCompleted)
            return Task.CompletedTask;

        //  Wrap DoWork() in Task.Run() to allow app to start without finishing this work.
        Task.Run(async () => { await DoWork(); }, cancellationToken);

        _workCompleted = true;
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(NewestStoriesBackgroundService)} stopped.");
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Gets the newest stories.
    /// </summary>
    private async Task DoWork()
    {
        _logger.LogInformation($"{nameof(NewestStoriesBackgroundService)} is working.");

        using var scope = _serviceProvider.CreateScope();
        var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();
        await storiesService.GetNewestStories();
    }
}