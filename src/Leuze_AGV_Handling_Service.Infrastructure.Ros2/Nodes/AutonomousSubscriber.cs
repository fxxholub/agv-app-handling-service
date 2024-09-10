using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

public class AutonomousSubscriber : BackgroundService, IAutonomousMessageReceiver
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AutonomousSubscriber> _logger;
    
    private readonly IRclSubscription<Ros2CommonMessages.Std.String> _mapSubscriber;
    public AutonomousSubscriber(IServiceProvider serviceProvider, ILogger<AutonomousSubscriber> logger)
    {
        _serviceProvider = serviceProvider;
        
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_autonomous_sub");
        
        _mapSubscriber = node.CreateSubscription<Ros2CommonMessages.Std.String>("/map");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var msg in _mapSubscriber.ReadAllAsync(cancellationToken))
        {
            await ReceiveMap(new MapDTO(msg.Data));
        }
    }

    public async Task ReceiveMap(MapDTO map)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var messageChannel = scope.ServiceProvider.GetRequiredService<IAutonomousMessageChannel>();
            await messageChannel.ReceiveMap(map);
        }
    }
}