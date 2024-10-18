using Leuze_AGV_Handling_Service.Core.Messages.DTOs;

namespace Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;

public interface IAutonomousMessageReceiver
{
    public Task ReceiveMap(MapDto map);
}