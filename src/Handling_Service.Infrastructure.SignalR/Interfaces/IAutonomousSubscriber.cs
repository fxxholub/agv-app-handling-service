using Handling_Service.UseCases.Messaging.DTOs;

namespace Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IAutonomousSubscriber
{
    public Task SubscribeMap(MapDto map);
}