using Leuze_AGV_Handling_Service.Core.Messages.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

public interface IAutonomousMessageReceiver
{
    public Task ReceiveMap(MapDTO map);
}