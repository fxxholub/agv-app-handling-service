
using Leuze_AGV_Handling_Service.Core.Messages.Events;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Messages.Handlers;

internal class SendJoyHandler(
  IAutonomousMessageSendForwarder forwarder
  ) : INotificationHandler<SendJoyEvent>
{
  public async Task Handle(SendJoyEvent messageEvent, CancellationToken cancellationToken)
  {
    await forwarder.SendJoy(messageEvent.MessageDto);
  }
}
