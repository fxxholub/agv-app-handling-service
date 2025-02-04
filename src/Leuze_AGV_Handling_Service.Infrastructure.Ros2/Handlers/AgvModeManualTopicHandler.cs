using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Handlers;

public class AgvModeManualTopicHandler(IManualPublisher manualPublisher) : INotificationHandler<AgvModeManualTopic>
{
    public async Task Handle(AgvModeManualTopic mode, CancellationToken cancellationToken)
    {
        await manualPublisher.PublishAgvModeManualTopic();
    }
}