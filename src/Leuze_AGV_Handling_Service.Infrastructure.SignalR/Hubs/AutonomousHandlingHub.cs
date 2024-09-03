using Leuze_AGV_Handling_Service.UseCases.Messages.AutonomousMessages.Joy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

// [ApiVersion(1)]
// [SignalRHub(path: "api/v{v:apiVersion}/signalr/handling-hub")]
[SignalRHub(path: "/api/v1/signalr/autonomous")]
public class AutonomousHandlingHub(IMediator mediator) : Hub<IAutonomousHandlingHub>
{
    public async Task SendJoy(string message)
    {
        // client receives echo message
        await mediator.Send(new SendJoyCommand(message));
    }
}