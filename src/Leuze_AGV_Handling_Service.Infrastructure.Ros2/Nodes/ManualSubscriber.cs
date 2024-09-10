using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

public class ManualSubscriber : BackgroundService, IAutonomousMessageReceiver
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ManualSubscriber> _logger;
    
    private readonly IRclSubscription<Ros2CommonMessages.Std.String> _mapSubscriber;
    public ManualSubscriber(IServiceProvider serviceProvider, ILogger<ManualSubscriber> logger)
    {
        _serviceProvider = serviceProvider;
        
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_manual_sub");
        
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