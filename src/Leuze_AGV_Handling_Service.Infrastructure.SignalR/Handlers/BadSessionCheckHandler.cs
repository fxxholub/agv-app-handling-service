using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.Notifications.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Handlers;

public class BadSessionCheckHandler(
    IAutonomousClientNotifier autonomousNotifier,
    IManualClientNotifier manualNotifier,
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

        if (session.Value.HandlingMode == HandlingMode.Autonomous)
        {
            await autonomousNotifier.ReceiveSessionUnexpectedEnd(
                "Autonomous Session Check resulted in false, Session ended.");
        }
        if (session.Value.HandlingMode == HandlingMode.Manual)
        {
            await manualNotifier.ReceiveSessionUnexpectedEnd(
                "Manual Session Check resulted in false, Session ended.");
        }
    }
}