using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;
using SignalRSwaggerGen.Naming;

namespace Leuze_AGV_Handling_Service.WebAPI.Hubs;

// [ApiVersion(1)]
// [SignalRHub(path: "api/v{v:apiVersion}/signalr/handling-hub")]
[SignalRHub(path: "/api/v1/signalr/handling-hub")]
public class HandlingHub : Hub<IHandlingHub>
{
    private static string? _connectedClientId = null;
    
    [SignalRHidden]
    public override async Task OnConnectedAsync() {
        if (_connectedClientId is null)
        {
            _connectedClientId = Context.ConnectionId;
            // TODO more complex check - if session id and session key matches
            // TODO let in everyone, but some messages are protected only for certain client
            await Clients.Caller.ReceiveMessage($"You are connected as {Context.ConnectionId}");
        }
        else
        {
            await Clients.Caller.ReceiveMessage("Another client is already connected");
            Context.Abort();
        }
    }
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.ConnectionId == _connectedClientId)
        {
            _connectedClientId = null;
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendCustomMessage(string messageText)
    {
        if (Context.ConnectionId != _connectedClientId)
        {
            await Clients.Caller.ReceiveMessage("Error You are not the connected client.");
            return;
        }
        
        await Clients.Caller.ReceiveMessage($"{Context.ConnectionId}: echo: '{messageText}'");
    }
}