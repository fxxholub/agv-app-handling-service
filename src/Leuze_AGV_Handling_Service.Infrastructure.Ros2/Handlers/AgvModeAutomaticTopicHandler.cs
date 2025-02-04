using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Handlers;

public class AgvModeAutomaticTopicHandler(IManualPublisher manualPublisher) : INotificationHandler<AgvModeAutomaticTopic>
{
    public async Task Handle(AgvModeAutomaticTopic mode, CancellationToken cancellationToken)
    {
        await manualPublisher.PublishAgvModeManualTopic();
    }
}