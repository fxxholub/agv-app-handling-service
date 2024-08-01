using Leuze_AGV_Handling_Service.Core.ContributorAggregate.Events;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Handlers;

internal class SessionStartedHandler(
  ILogger<SessionStartedHandler> logger
  ) : INotificationHandler<SessionStartedEvent>
{
  public async Task Handle(SessionStartedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Session Started event for {sessionId}", domainEvent.SessionId);

    // TODO handle SignalR connection open here
    await Task.Delay(10);
  }
}
