
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Handlers;

/// <summary>
/// TODO
/// </summary>
/// <param name="logger"></param>
internal class SessionEndedHandler(
  IAutonomousMessageChannel autonomousChannel,
  IManualMessageChannel manualChannel,
  ILogger<SessionEndedHandler> logger
  ) : INotificationHandler<SessionEndedEvent>
{
  public async Task Handle(SessionEndedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Session Ended event for {sessionId}", domainEvent.SessionId);

    if (domainEvent.sessionMode is HandlingMode.Autonomous)
    {
      await autonomousChannel.Disable();
    } else if (domainEvent.sessionMode is HandlingMode.Manual)
    {
      await manualChannel.Disable();
    }
  }
}
