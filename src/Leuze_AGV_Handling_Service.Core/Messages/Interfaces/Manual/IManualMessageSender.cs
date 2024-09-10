using Leuze_AGV_Handling_Service.Core.Messages.DTOs;

namespace Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;

public interface IManualMessageSender
{
    public Task SendJoy(JoyDTO joy);
}