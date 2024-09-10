
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Handlers;

/// <summary>
/// TODO
/// </summary>
/// <param name="logger"></param>
internal class SessionStartedHandler(
  IAutonomousMessageChannel autonomousChannel,
  IManualMessageChannel manualChannel,
  ILogger<SessionStartedHandler> logger
  ) : INotificationHandler<SessionStartedEvent>
{
  public async Task Handle(SessionStartedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Session Started event for {sessionId}", domainEvent.SessionId);

    if (domainEvent.sessionMode is HandlingMode.Autonomous)
    {
      await autonomousChannel.Enable();
    } else if (domainEvent.sessionMode is HandlingMode.Manual)
    {
      await manualChannel.Enable();
    }
  }
}
