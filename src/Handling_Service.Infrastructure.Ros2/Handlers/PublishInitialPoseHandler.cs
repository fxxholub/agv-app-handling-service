using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class PublishInitialPoseHandler(IPublisherNode publisherNode) : INotificationHandler<PublishInitialPoseTopic>
{
    public async Task Handle(PublishInitialPoseTopic message, CancellationToken cancellationToken)
    {
        await publisherNode.PublishInitialPose(message.Data);
    }
}