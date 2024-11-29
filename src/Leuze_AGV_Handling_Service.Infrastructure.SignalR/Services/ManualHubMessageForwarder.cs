using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Services;

/// <summary>
/// Service forwarding manual messages from channel to the hub server side (server -> client).
/// </summary>
/// <param name="hubContext"></param>
public class ManualHubMessageForwarder(
    IHubContext<ManualHandlingHub, IManualHandlingHub> hubContext
    )
    : IManualSubscriber, IManualClientNotifier
{
    public async Task ReceiveSessionUnexpectedEnd(string errorMessage)
    {
        await hubContext.Clients.All.ReceiveSessionUnexpectedEnd(errorMessage);
    }
    
    // ROS stuff ////////////////////////////////////////////////////////////////////////////////
    
    public async Task SubscribeMapTopic(MapDto map)
    {
        await hubContext.Clients.All.SusbcribeMapTopic(map);
    }
}