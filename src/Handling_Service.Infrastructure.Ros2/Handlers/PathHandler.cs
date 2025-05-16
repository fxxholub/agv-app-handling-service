using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class PathHandler(IPublisherNode publisherNode) : INotificationHandler<PathTopic>
{
    public async Task Handle(PathTopic message, CancellationToken cancellationToken)
    {
        await publisherNode.PublishPath(message.data);
    }
}