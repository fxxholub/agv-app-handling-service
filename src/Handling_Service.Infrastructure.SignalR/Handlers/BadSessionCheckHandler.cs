using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using Handling_Service.UseCases.Session.Notifications.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Handling_Service.Infrastructure.SignalR.Handlers;

public class BadSessionCheckHandler(
    IClientNotifier notifier,
    ILogger<BadSessionCheckHandler> logger,
    IMediator mediator
    ) : INotificationHandler<BadSessionCheckEvent>
{
    public async Task Handle(BadSessionCheckEvent notification, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new GetSessionQuery(notification.SessionId), cancellationToken);

        if (!session.IsSuccess)
        {
            logger.LogError("Bad Session Check SignalR handler failed.");
            return;
        }

        await notifier.SessionUnexpectedEnd(
            "Session Check resulted in false, Session ended.");
    }
}