using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 subscriber of manual messages.
/// </summary>
public class ManualSubscriber : BackgroundService, IManualSubscriber
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IRclSubscription<Ros2CommonMessages.Std.String> _mapSubscriber;
    public ManualSubscriber(IServiceProvider serviceProvider, ILogger<ManualSubscriber> logger)
    {
        _serviceProvider = serviceProvider;

        logger.LogInformation($"Handling Ros2 handling_service_manual_sub node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_manual_sub");
        
        _mapSubscriber = node.CreateSubscription<Ros2CommonMessages.Std.String>("/map");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var msg in _mapSubscriber.ReadAllAsync(cancellationToken))
        {
            
        }
    }
}