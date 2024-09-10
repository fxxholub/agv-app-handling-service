using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Events;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

public interface IMessageForwarder
{
    public void Enable();
    public void Disable();

    public bool IsEnabled();
}