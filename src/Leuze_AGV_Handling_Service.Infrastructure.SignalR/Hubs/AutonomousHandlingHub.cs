using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Autonomous handling mode Hub
/// </summary>
/// <param name="messageChannel"></param>
[SignalRHub(path: "/api/v1/signalr/autonomous")]
public class AutonomousHandlingHub(/*IAutonomousMessageChannel messageChannel*/) : Hub<IAutonomousHandlingHub>/*, IAutonomousMessageSender*/
{
    public async Task Dummy()
    {
        await Task.Delay(1000);
    }
}