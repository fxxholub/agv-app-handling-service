using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

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