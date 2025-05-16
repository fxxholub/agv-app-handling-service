using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class AgvModeHandler(IPublisherNode publisherNode) : INotificationHandler<AgvModeTopic>
{
    public async Task Handle(AgvModeTopic message, CancellationToken cancellationToken)
    {
        await publisherNode.PublishAgvMode(message.data);
    }
}