using Leuze_AGV_Handling_Service.Core.Messages.DTOs;

namespace Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;

public interface IAutonomousMessageSender
{
    public Task SendJoy(JoyDTO joy);
}