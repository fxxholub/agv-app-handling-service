using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Services;

/// <summary>
/// Service forwarding manual messages from channel to the hub server side (server -> client).
/// </summary>
/// <param name="hubContext"></param>
public class ManualHubMessageForwarder(
    IHubContext<ManualHandlingHub, IManualHandlingHub> hubContext
    )
    : IManualMessageReceiver
{
    public async Task ReceiveMap(MapDto map)
    {
        await hubContext.Clients.All.ReceiveMap(map);
    }
}