using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using MediatR;

namespace Leuze_AGV_Handling_Service.Core.Messages.Events;

public class ReceiveMapEvent(MapDTO mapDto) : INotification
{
  public MapDTO MessageDto { get; set; } = mapDto;
}
