using Leuze_AGV_Handling_Service.Core.Events;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Handlers;

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

    if (domainEvent.SessionMode is HandlingMode.Autonomous)
    {
      await autonomousChannel.Enable();
    } else if (domainEvent.SessionMode is HandlingMode.Manual)
    {
      await manualChannel.Enable();
    }
  }
}
