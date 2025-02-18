using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using IPublisher = Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class CmdVelHandler(IPublisher publisher) : INotificationHandler<CmdVel>
{
    public async Task Handle(CmdVel message, CancellationToken cancellationToken)
    {
        await publisher.PublishCmdVel(message.data);
    }
}