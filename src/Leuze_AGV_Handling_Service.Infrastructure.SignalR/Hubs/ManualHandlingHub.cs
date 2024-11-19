using System.Runtime.CompilerServices;
using Ardalis.Result;
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
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Manual handling mode Hub
/// </summary>
/// <param name="messageChannel"></param>
[SignalRHub(path: "/api/v1/signalr/manual")]
public class ManualHandlingHub(
    IMediator mediator,
    ILogger<ManualHandlingHub> logger,
    IManualMessageChannel messageChannel
    ) : Hub<IManualHandlingHub>, IManualMessageSender
{
    // HUB ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        switch (result.Status)
        {
            case (ResultStatus.Ok):
                return;
            default:
                logger.LogError($"Manual Session Leave failed: {result.Status}. Leaved forcefully.");
                break;
        } 

        await base.OnDisconnectedAsync(exception);
    }
    
    // Session CRUD ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task<SessionResponseModel> SendCreateSession()
    {
        // create the session entity
        var createResult = await mediator.Send(new CreateSessionCommand(
            HandlingMode.Manual, Lifespan.Exclusive));
        
        switch (createResult.Status)
        {
            case (ResultStatus.Ok):
                break;
            default:
                logger.LogError       ($"Manual Session Create failed: {createResult.Status}. Unknown reason.");
                throw new HubException($"Manual Session Create failed: {createResult.Status}. Unknown reason.");
        }
        
        // return the created entity
        var resultEntity = await mediator.Send(new GetSessionQuery(createResult.Value));
        
        switch (createResult.Status)
        {
            case (ResultStatus.Ok):
                break;
            default:
                logger.LogError       ($"Manual Session Create failed: {createResult.Status}. Could not return the created entity.");
                throw new HubException($"Manual Session Create failed: {createResult.Status}. Could not return the created entity.");
        }

        return SessionResponseModel.ToModel(resultEntity.Value);
    }
    
    // Session Actions ////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task SendStartSession(int sessionId)
    {
        var result = await mediator.Send(new StartSessionCommand(sessionId, Context.ConnectionId));
        switch (result.Status)
        {
            case (ResultStatus.Ok):
                return;
            case (ResultStatus.Unauthorized):
                logger.LogWarning     ($"Manual Session Start failed: {result.Status}.");
                throw new HubException($"Manual Session Start failed: {result.Status}.");
            case (ResultStatus.Conflict):
                logger.LogWarning     ($"Manual Session Start failed: {result.Status}. Another Session is probably running.");
                throw new HubException($"Manual Session Start failed: {result.Status}. Another Session is probably running.");
            default:
                logger.LogError       ($"Manual Session Start failed: {result.Status}. Unknown reason.");
                throw new HubException($"Manual Session Start failed: {result.Status}. Unknown reason.");
        }
    }

    public async Task SendEndSession(int sessionId)
    {
        var result = await mediator.Send(new EndSessionCommand(sessionId, Context.ConnectionId));
        switch (result.Status)
        {
            case (ResultStatus.Ok):
                return;
            case (ResultStatus.Unauthorized):
                logger.LogWarning     ($"Manual Session End failed: {result.Status}.");
                throw new HubException($"Manual Session End failed: {result.Status}.");
            default:
                logger.LogError       ($"Manual Session End failed: {result.Status}. Unknown reason.");
                throw new HubException($"Manual Session End failed: {result.Status}. Unknown reason.");
        }
    }
    
    public async Task SendLeaveSession(int sessionId)
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        switch (result.Status)
        {
            case (ResultStatus.Ok):
                return;
            case (ResultStatus.Unauthorized):
                logger.LogWarning     ($"Manual Session End failed: {result.Status}.");
                throw new HubException($"Manual Session End failed: {result.Status}.");
            default:
                logger.LogError       ($"Manual Session End failed: {result.Status}. Unknown reason.");
                throw new HubException($"Manual Session End failed: {result.Status}. Unknown reason.");
        }
    }
    
    // ROS Messages ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task SendJoy(JoyDto joy)
    {
        if (mediator.Send(new IsCurrentConnectionQuery(Context.ConnectionId)).Result.Value)
            await messageChannel.SendJoy(joy);
    }
}