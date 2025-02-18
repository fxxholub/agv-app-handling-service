using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using IPublisher = Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class AgvModeHandler(IPublisher publisher) : INotificationHandler<AgvMode>
{
    public async Task Handle(AgvMode message, CancellationToken cancellationToken)
    {
        await publisher.PublishAgvMode(message.mode);
    }
}