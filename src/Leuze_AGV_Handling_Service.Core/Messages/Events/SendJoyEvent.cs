using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using MediatR;

namespace Leuze_AGV_Handling_Service.Core.Messages.Events;

public class SendJoyEvent(JoyDTO joyDto) : INotification
{
  public JoyDTO MessageDto { get; set; } = joyDto;
}
