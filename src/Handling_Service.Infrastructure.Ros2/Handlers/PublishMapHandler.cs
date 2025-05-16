using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class PublishMapHandler(IPublisherNode publisherNode) : INotificationHandler<PublishMapTopic>
{
    public async Task Handle(PublishMapTopic message, CancellationToken cancellationToken)
    {
        await publisherNode.PublishMap(message.Data);
    }
}