using Handling_Service.UseCases.Messaging.DTOs;

namespace Handling_Service.Infrastructure.SignalR.Hubs;

public interface IHandlingHub
{
    Task SessionUnexpectedEnd(string errorMessage);
    
    public Task SubscribeMap(MapDto map);
}