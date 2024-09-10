using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Services;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Contexts;

public class ManualHubReceiveForwarder(
    IHubContext<ManualHandlingHub, IManualHandlingHub> hubContext
    )
    : MessageForwarderBase, IManualMessageReceiveForwarder
{
    public async Task ReceiveMap(MapDTO map)
    {
        if (IsEnabled())
        {
            await hubContext.Clients.All.ReceiveMap(map);
        }
    }
}