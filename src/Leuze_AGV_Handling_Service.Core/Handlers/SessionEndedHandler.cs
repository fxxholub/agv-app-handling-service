
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Handlers;

/// <summary>
/// TODO
/// </summary>
/// <param name="logger"></param>
internal class SessionEndedHandler(
  ILogger<SessionEndedHandler> logger
  ) : INotificationHandler<SessionEndedEvent>
{
  public async Task Handle(SessionEndedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Session Ended event for {sessionId}", domainEvent.SessionId);

    // TODO handle SignalR connection close here
    await Task.Delay(10);
  }
}
