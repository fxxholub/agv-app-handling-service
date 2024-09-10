using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Services;

public class AutonomousHubMessageForwarder(
    IHubContext<AutonomousHandlingHub, IAutonomousHandlingHub> hubContext
    )
    : IAutonomousMessageReceiver
{
    public async Task ReceiveMap(MapDTO map)
    {
        await hubContext.Clients.All.ReceiveMap(map);
    }
}