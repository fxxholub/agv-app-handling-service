using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public class HandlingHubBase: Hub
{
    private string? _sessionOwnerId = null;
    
    [SignalRHidden]
    public override async Task OnConnectedAsync() {
        if (_sessionOwnerId is null)
        {
            _sessionOwnerId = Context.ConnectionId;
            // TODO more complex check - if session id and session key matches
            // TODO let in everyone, but some messages are protected only for certain client
            // await Clients.Caller.ReceiveMessage($"You are connected as {Context.ConnectionId}");
        }
        // else
        // {
        //     await Clients.Caller.ReceiveMessage("Another client is already connected");
        //     Context.Abort();
        // }
        await base.OnConnectedAsync();
    }
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.ConnectionId == _sessionOwnerId)
        {
            _sessionOwnerId = null;
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    [SignalRHidden]
    public bool IsClientSessionOwner()
    {
        return Context.ConnectionId == _sessionOwnerId;
    }
}