using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public interface IManualHandlingHub
{
    Task SessionUnexpectedEnd(string errorMessage);
    
    public Task SubscribeMap(MapDto map);
}