using MediatR;
using IPublisher = Handling_Service.Infrastructure.Ros2.Interfaces.IPublisher;
using Path = Handling_Service.UseCases.Messaging.Topics.Path;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class PathHandler(IPublisher publisher) : INotificationHandler<Path>
{
    public async Task Handle(Path message, CancellationToken cancellationToken)
    {
        await publisher.PublishPath(message.path);
    }
}