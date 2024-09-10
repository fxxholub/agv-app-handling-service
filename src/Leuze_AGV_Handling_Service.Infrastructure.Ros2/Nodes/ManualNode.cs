using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Events;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

public class ManualNode : BackgroundService, IAutonomousMessageSender, IAutonomousMessageReceiver, IManualMessageSender
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ManualNode> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _joyPublisher;
    private readonly IRclSubscription<Ros2CommonMessages.Std.String> _mapSubscriber;
    public ManualNode(IServiceProvider serviceProvider, ILogger<ManualNode> logger)
    {
        _serviceProvider = serviceProvider;
        
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_autonomous");

        _joyPublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/joy");
        _mapSubscriber = node.CreateSubscription<Ros2CommonMessages.Std.String>("/map");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var msg in _mapSubscriber.ReadAllAsync(cancellationToken))
        {
            await ReceiveMap(new MapDTO(msg.Data));
        }
    }

    public async Task SendJoy(JoyDTO message)
    {
        var msg = new Ros2CommonMessages.Std.String(message.Something);
        await _joyPublisher.PublishAsync(msg);
    }

    public async Task ReceiveMap(MapDTO message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Publish(new ReceiveMapEvent(message));
        }
    }
}