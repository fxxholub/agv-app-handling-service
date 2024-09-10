using Leuze_AGV_Handling_Service.Core.Events;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Handlers;

/// <summary>
/// Handler effectively disabling communication of the message channel.
/// </summary>
/// <param name="autonomousChannel"></param>
/// <param name="manualChannel"></param>
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

    if (domainEvent.SessionMode is HandlingMode.Autonomous)
    {
      await autonomousChannel.Disable();
    } else if (domainEvent.SessionMode is HandlingMode.Manual)
    {
      await manualChannel.Disable();
    }
  }
}
