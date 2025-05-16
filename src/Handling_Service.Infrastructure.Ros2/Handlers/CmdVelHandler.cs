using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class CmdVelHandler(IPublisherNode publisherNode) : INotificationHandler<CmdVelTopic>
{
    public async Task Handle(CmdVelTopic message, CancellationToken cancellationToken)
    {
        await publisherNode.PublishCmdVel(message.data);
    }
}