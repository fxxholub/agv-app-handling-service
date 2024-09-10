using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Hub for all autonomous messages.
/// </summary>
/// <param name="messageChannel"></param>
// [ApiVersion(1)]
// [SignalRHub(path: "api/v{v:apiVersion}/signalr/handling-hub")]
[SignalRHub(path: "/api/v1/signalr/autonomous")]
public class AutonomousHandlingHub(IAutonomousMessageChannel messageChannel) : Hub<IAutonomousHandlingHub>, IAutonomousMessageSender
{
    public async Task SendJoy(JoyDTO joy)
    {
        await messageChannel.SendJoy(joy);
    }
}