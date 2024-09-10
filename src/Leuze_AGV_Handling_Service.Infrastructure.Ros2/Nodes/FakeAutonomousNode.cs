using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;
public class FakeAutonomousNode : IHostedService, IDisposable/*, IAutonomousMessageSender, IAutonomousMessageReceiver*/
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FakeAutonomousNode> _logger;
    private Timer? _timer = null;

    public FakeAutonomousNode(IServiceProvider serviceProvider, ILogger<FakeAutonomousNode> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _logger.LogInformation("Fake Handling Ros2 node started.");
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Fake Handling Ros2 node started.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(2.5));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        await Task.Delay(1000);
        // await ReceiveMap(new MapDTO("Fake Ros2 node does work!"));
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Fake Ros2 node stopped.");

        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    // public async Task SendJoy(JoyDTO message)
    // {
    //     _logger.LogDebug("Fake Ros2 node SendJoy.");
    //     await Task.Delay(100);
    // }
    //
    // public async Task ReceiveMap(MapDTO message)
    // {
    //     _logger.LogDebug("Fake Ros2 node ReceiveMap.");
    //
    //     using (var scope = _serviceProvider.CreateScope())
    //     {
    //         var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    //         await mediator.Publish(new ReceiveMapEvent(message));
    //     }
    // }
}
