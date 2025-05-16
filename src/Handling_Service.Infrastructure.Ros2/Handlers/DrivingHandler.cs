using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class DrivingHandler(IPublisherNode publisherNode) : INotificationHandler<DrivingTopic>
{
    public async Task Handle(DrivingTopic message, CancellationToken cancellationToken)
    {
        await publisherNode.PublishDriving(message.data);
    }
}