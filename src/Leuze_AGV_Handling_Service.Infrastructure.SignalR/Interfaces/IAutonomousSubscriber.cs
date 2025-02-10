using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IAutonomousSubscriber
{
    public Task SubscribeMap(MapDto map);
}