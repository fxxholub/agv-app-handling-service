using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IManualSubscriber
{
    public Task SubscribeMapTopic(MapDto map);
}