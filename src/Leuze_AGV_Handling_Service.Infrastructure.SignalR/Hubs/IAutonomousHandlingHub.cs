using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public interface IAutonomousHandlingHub
{
    Task ReceiveSessionUnexpectedEnd(string errorMessage);
    
    public Task SubscribeMapTopic(MapDto map);
}