using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Services;

/// <summary>
/// Service forwarding autonomous messages from channel to the hub server side (server -> client).
/// </summary>
/// <param name="hubContext"></param>
public class AutonomousHubMessageForwarder(
    IHubContext<AutonomousHandlingHub, IAutonomousHandlingHub> hubContext
    )
    : IAutonomousSubscriber, IAutonomousClientNotifier
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