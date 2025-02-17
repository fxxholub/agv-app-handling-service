using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using Interfaces_IPublisher = Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;
using IPublisher = Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class AgvModeHandler(Interfaces_IPublisher publisher) : INotificationHandler<AgvMode>
{
    public async Task Handle(AgvMode message, CancellationToken cancellationToken)
    {
        await publisher.PublishAgvMode(message.mode);
    }
}