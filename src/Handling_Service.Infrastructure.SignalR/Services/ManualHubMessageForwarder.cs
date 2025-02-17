using Handling_Service.Infrastructure.SignalR.Hubs;
using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.UseCases.Messaging.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Handling_Service.Infrastructure.SignalR.Services;

/// <summary>
/// Service forwarding manual messages from channel to the hub server side (server -> client).
/// </summary>
/// <param name="hubContext"></param>
public class ManualHubMessageForwarder(
    IHubContext<ManualHandlingHub, IManualHandlingHub> hubContext
    )
    : IManualSubscriber, IManualClientNotifier
{
    public async Task SessionUnexpectedEnd(string errorMessage)
    {
        await hubContext.Clients.All.SessionUnexpectedEnd(errorMessage);
    }
    
    // ROS stuff ////////////////////////////////////////////////////////////////////////////////
    
    public async Task SubscribeMap(MapDto map)
    {
        await hubContext.Clients.All.SubscribeMap(map);
    }
}