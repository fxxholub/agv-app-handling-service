using Docker.DotNet;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Session.Notifications.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.SessionServices;

/// <summary>
/// Watchdog hosted service. Periodically checks actively running session, if its processes did not fail.
/// </summary>
public class SessionWatchdogService(
    IServiceProvider serviceProvider,
    ILogger<SessionWatchdogService> logger
    ) : IHostedService, IDisposable, ISessionWatchdogService
{
    private const int WatchPeriod = 1;
    private int? _watchedSessionId = null;
    
    private Timer? _timer = null;

    public async Task StartWatching(int sessionId)
    {
        _watchedSessionId = sessionId;
        await StartAsync(new CancellationToken());
    }

    public async Task StopWatching()
    {
        _watchedSessionId = null;
        await StopAsync(new CancellationToken());
    }
    
    public async Task Watch()
    {
        using var scope = serviceProvider.CreateScope();
        var checkService = scope.ServiceProvider.GetRequiredService<ICheckSessionService>();
        var endService = scope.ServiceProvider.GetRequiredService<IEndSessionService>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        if (_watchedSessionId is null)
        {
            await StopWatching();
            return;
        }

        try
        {
            var checkResult = await checkService.CheckSession(_watchedSessionId.Value);

            if (!checkResult.IsSuccess)
                logger.LogError("Session Watchdog check failed.");

            // check ok return
            if (checkResult.Value) return;
            await mediator.Publish(new BadSessionCheckEvent(_watchedSessionId.Value));

            var endResult = await endService.EndSession(_watchedSessionId.Value);

            if (!endResult.IsSuccess)
                logger.LogError("Session Watchdog end on bad check failed.");

            await StopWatching();
        }
        catch (DbUpdateConcurrencyException)
        {
            logger.LogWarning($"Session Watchdog check exception. Session no longer exists.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Session Watchdog check error: {ex.Message}");
        }
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Session Watchdog service started, will watch every {WatchPeriod} s.");

        _timer = new Timer(async _ => await Watch(), null, TimeSpan.Zero,
            TimeSpan.FromSeconds(WatchPeriod));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        
        logger.LogInformation("Session Watchdog service stopped.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _watchedSessionId = null;
    }
}