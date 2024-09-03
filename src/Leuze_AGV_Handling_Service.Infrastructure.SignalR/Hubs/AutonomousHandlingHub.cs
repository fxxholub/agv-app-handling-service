using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;
using SignalRSwaggerGen.Naming;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

// [ApiVersion(1)]
// [SignalRHub(path: "api/v{v:apiVersion}/signalr/handling-hub")]
[SignalRHub(path: "/api/v1/signalr/autonomous")]
public class AutonomousHandlingHub : Hub<IAutonomousHandlingHub>
{
    // // client Sends custom message
    // public async Task SendCustomMessage(string messageText)
    // {
    //     // client receives echo message
    //     await Clients.Caller.ReceiveMessage($"{Context.ConnectionId}: echo: '{messageText}'");
    // }
}