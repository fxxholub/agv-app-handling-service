using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.End;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Leave;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Start;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Manual handling mode Hub
/// </summary>
/// <param name="messageChannel"></param>
[SignalRHub(path: "/api/v1/signalr/manual")]
public class ManualHandlingHub(
    IMediator mediator,
    // ILogger<ManualHandlingHub> logger,
    IManualMessageChannel messageChannel
    ) : Hub<IManualHandlingHub>, IManualMessageSender
{
    // HUB ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        if (!result.IsSuccess) throw new HubException("Manual Session Leave error.");

        await base.OnDisconnectedAsync(exception);
    }
    
    // Session CRUD ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task<SessionResponseModel> SendCreateSession()
    {
        var createResult = await mediator.Send(new CreateSessionCommand(
            HandlingMode.Manual, Lifespan.Exclusive));
        
        if (!createResult.IsSuccess) throw new HubException("Manual Session Create error.");
        
        var resultEntity = await mediator.Send(new GetSessionQuery(createResult.Value));
        if (!resultEntity.IsSuccess) throw new HubException("Manual Session Create error - could not get session response data");

        return SessionResponseModel.ToModel(resultEntity.Value);
    }
    
    // Session Actions ////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task SendStartSession(int sessionId)
    {
        var result = await mediator.Send(new StartSessionCommand(sessionId, Context.ConnectionId));
        if (!result.IsSuccess) throw new HubException("Manual Session Start error.");
    }

    public async Task SendEndSession(int sessionId)
    {
        var result = await mediator.Send(new EndSessionCommand(sessionId, Context.ConnectionId));
        if (!result.IsSuccess) throw new HubException("Manual Session End error.");
    }
    
    public async Task SendLeaveSession(int sessionId)
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        if (!result.IsSuccess) throw new HubException("Manual Session leave error.");
    }
    
    // ROS Messages ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task SendJoy(JoyDto joy)
    {
        if (mediator.Send(new IsCurrentConnectionQuery(Context.ConnectionId)).Result.Value)
            await messageChannel.SendJoy(joy);
    }
}