using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

// [ApiVersion(1)]
// [SignalRHub(path: "api/v{v:apiVersion}/signalr/handling-hub")]
[SignalRHub(path: "/api/v1/signalr/manual")]
public class ManualHandlingHub(IManualMessageChannel messageChannel) : Hub<IManualHandlingHub>, IManualMessageSender
{
    public async Task SendJoy(JoyDTO joy)
    {
        await messageChannel.SendJoy(joy);
    }
}