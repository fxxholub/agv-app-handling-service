using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 subscriber of autonomous messages.
/// </summary>
public class AutonomousSubscriber : BackgroundService, IAutonomousSubscriber
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AutonomousSubscriber> _logger;
    
    private readonly IRclSubscription<Ros2CommonMessages.Nav.OccupancyGrid> _mapSubscriber;
    public AutonomousSubscriber(IServiceProvider serviceProvider, ILogger<AutonomousSubscriber> logger)
    {
        _serviceProvider = serviceProvider;
            
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 handling_service_autonomous_sub node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_autonomous_sub");
        
        _mapSubscriber = node.CreateSubscription<Ros2CommonMessages.Nav.OccupancyGrid>("/map");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var msg in _mapSubscriber.ReadAllAsync(cancellationToken))
        {
            await SubscribeMapTopic(OccupancyGrid2MapDto(msg));
        }
    }

    public async Task SubscribeMapTopic(MapDto map)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(new MapTopic(map));
    }

    private MapDto OccupancyGrid2MapDto(Ros2CommonMessages.Nav.OccupancyGrid occupancyGrid)
    {
        return new MapDto(
            Resolution: occupancyGrid.Info.Resolution,
            Width: occupancyGrid.Info.Width,
            Height: occupancyGrid.Info.Height,
            Origin: new PoseDto(
                Position: (
                    occupancyGrid.Info.Origin.Position.X,
                    occupancyGrid.Info.Origin.Position.Y,
                    occupancyGrid.Info.Origin.Position.Z
                ),
                Orientation: (
                    occupancyGrid.Info.Origin.Orientation.X,
                    occupancyGrid.Info.Origin.Orientation.Y,
                    occupancyGrid.Info.Origin.Orientation.Z,
                    occupancyGrid.Info.Origin.Orientation.W
                )
            ),
            Data: occupancyGrid.Data
        );
    }
}