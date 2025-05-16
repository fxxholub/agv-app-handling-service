using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 subscriber of manual messages.
/// </summary>
public class SubscriberNode : BackgroundService, ISubscriberNode
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SubscriberNode> _logger;
    
    private readonly IRclSubscription<OccupancyGrid> _mapSubscriber;
    private readonly IRclSubscription<PoseStamped> _robotPoseSubscriber;
    public SubscriberNode(IServiceProvider serviceProvider, ILogger<SubscriberNode> logger)
    {
        _serviceProvider = serviceProvider;
            
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 handling_service_manual_sub node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_subscriber");
        
        _mapSubscriber = node.CreateSubscription<OccupancyGrid>("/map");
        _robotPoseSubscriber = node.CreateSubscription<PoseStamped>("/robot_pose");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Start both subscription tasks concurrently
        var mapTask = Task.Run(() => ProcessMapSubscription(cancellationToken), cancellationToken);
        var robotPoseTask = Task.Run(() => ProcessRobotPoseSubscription(cancellationToken), cancellationToken);
    
        // Wait for both tasks to complete (or cancel)
        await Task.WhenAll(mapTask, robotPoseTask);
    }
    
    private async Task ProcessMapSubscription(CancellationToken cancellationToken)
    {
        await foreach (var msg in _mapSubscriber.ReadAllAsync(cancellationToken))
        {
            await SubscribeMap(msg);
        }
    }
    
    private async Task ProcessRobotPoseSubscription(CancellationToken cancellationToken)
    {
        await foreach (var msg in _robotPoseSubscriber.ReadAllAsync(cancellationToken))
        {
            await SubscribeRobotPose(msg);
        }
    }

    public async Task SubscribeMap(OccupancyGrid data)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(new MapTopic(data));
    }

    public async Task SubscribeRobotPose(PoseStamped data)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(new RobotPoseTopic(data));
    }
}