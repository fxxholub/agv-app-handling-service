using Handling_Service.Infrastructure.SignalR.Hubs;
using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Models.Ros2;
using Microsoft.AspNetCore.SignalR;

namespace Handling_Service.Infrastructure.SignalR.Services;

/// <summary>
/// Service forwarding manual messages from channel to the hub server side (server -> client).
/// </summary>
/// <param name="hubContext"></param>
public class HubMessageForwarder(
    IHubContext<HandlingHub, IHandlingHub> hubContext
    )
    : IPushHub, IClientNotifier
{
    public async Task SessionUnexpectedEnd(string errorMessage)
    {
        await hubContext.Clients.All.SessionUnexpectedEnd(errorMessage);
    }
    
    // ROS stuff ////////////////////////////////////////////////////////////////////////////////
    
    public async Task SubscribeMap(OccupancyGridModel data)
    {
        await hubContext.Clients.All.SubscribeMap(data);
    }
    
    public async Task SubscribeRobotPose(PoseModel data)
    {
        await hubContext.Clients.All.SubscribeRobotPose(data);
    }
}