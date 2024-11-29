using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Handlers;

public class MapTopicHandler(IManualSubscriber manualSubscriber, IAutonomousSubscriber autonomousSubscriber) : INotificationHandler<MapTopic>
{
    public async Task Handle(MapTopic message, CancellationToken cancellationToken)
    {
        await manualSubscriber.SubscribeMapTopic(message.Map);
        await autonomousSubscriber.SubscribeMapTopic(message.Map);
    }
}