using Leuze_AGV_Handling_Service.Core.ContributorAggregate.Events;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Handlers;

internal class SessionDeletedHandler(
  ILogger<SessionDeletedHandler> logger
  ) : INotificationHandler<SessionDeletedEvent>
{
  public async Task Handle(SessionDeletedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Session Deleted event for {sessionId}", domainEvent.SessionId);

    // TODO handle SignalR connection close here
    await Task.Delay(10);

    // await emailSender.SendEmailAsync("to@test.com",
    //                                  "from@test.com",
    //                                  "Contributor Deleted",
    //                                  $"Contributor with id {domainEvent.ContributorId} was deleted.");
  }
}
