using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.SignalR.Handlers;

public class MapHandler(IManualSubscriber manualSubscriber, IAutonomousSubscriber autonomousSubscriber) : INotificationHandler<Map>
{
    public async Task Handle(Map message, CancellationToken cancellationToken)
    {
        await manualSubscriber.SubscribeMap(message.map);
        await autonomousSubscriber.SubscribeMap(message.map);
    }
}