using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using IPublisher = Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Handlers;

public class CmdVelHandler(IPublisher publisher) : INotificationHandler<CmdVel>
{
    public async Task Handle(CmdVel message, CancellationToken cancellationToken)
    {
        await publisher.PublishCmdVel(message.X, message.Y, message.W);
    }
}