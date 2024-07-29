using Leuze_AGV_Handling_Service.Core.ContributorAggregate.Events;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Handlers;

internal class SessionCreatedHandler(
  ILogger<SessionCreatedHandler> logger
  ) : INotificationHandler<SessionCreatedEvent>
{
  public async Task Handle(SessionCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Session Created event for {sessionId}", domainEvent.SessionId);

    // TODO handle SignalR connection open here
    await Task.Delay(10);

    // await emailSender.SendEmailAsync("to@test.com",
    //                                  "from@test.com",
    //                                  "Contributor Deleted",
    //                                  $"Contributor with id {domainEvent.ContributorId} was deleted.");
  }
}
