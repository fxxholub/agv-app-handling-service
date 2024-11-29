using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public interface IManualHandlingHub
{
    Task ReceiveSessionUnexpectedEnd(string errorMessage);
    
    public Task SubscribeMapTopic(MapDto map);
}