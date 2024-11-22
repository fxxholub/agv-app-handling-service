using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Handlers;

public class JoyTopicHandler(IManualPublisher manualPublisher) : INotificationHandler<JoyTopic>
{
    public async Task Handle(JoyTopic message, CancellationToken cancellationToken)
    {
        await manualPublisher.PublishJoyTopic(message.X, message.Y, message.W);
    }
}