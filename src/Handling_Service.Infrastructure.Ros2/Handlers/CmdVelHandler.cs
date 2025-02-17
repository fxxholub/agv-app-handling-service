using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using Interfaces_IPublisher = Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;
using IPublisher = Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class CmdVelHandler(Interfaces_IPublisher publisher) : INotificationHandler<CmdVel>
{
    public async Task Handle(CmdVel message, CancellationToken cancellationToken)
    {
        await publisher.PublishCmdVel(message.X, message.Y, message.W);
    }
}