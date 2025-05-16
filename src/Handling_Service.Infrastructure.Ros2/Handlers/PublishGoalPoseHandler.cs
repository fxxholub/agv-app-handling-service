using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class PublishGoalPoseHandler(IPublisherNode publisherNode) : INotificationHandler<PublishGoalPoseTopic>
{
    public async Task Handle(PublishGoalPoseTopic message, CancellationToken cancellationToken)
    {
        await publisherNode.PublishGoalPose(message.Data);
    }
}