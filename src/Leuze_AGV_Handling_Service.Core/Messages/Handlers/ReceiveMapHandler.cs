
using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Events;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Messages.Handlers;

internal class ReceiveMapHandler(
  IAutonomousMessageReceiveForwarder forwarder
  ) : INotificationHandler<ReceiveMapEvent>
{
  public async Task Handle(ReceiveMapEvent messageEvent, CancellationToken cancellationToken)
  {
    await forwarder.ReceiveMap(messageEvent.MessageDto);
  }
}
