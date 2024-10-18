using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.End;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Start;
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
    ISessionOwnershipService ownership,
    IManualMessageChannel messageChannel
    ) : Hub<IManualHandlingHub>, IManualMessageSender
{
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (await ownership.IsReservedByMe(Context.ConnectionId))
        {
            var sessionId = await ownership.GetSessionId();
            if (sessionId.HasValue)
            {
                var result = await mediator.Send(new EndSessionCommand(sessionId.Value));
                if (result.IsSuccess)
                {
                    var isFree = await ownership.Free(Context.ConnectionId);
                    if (!isFree) logger.LogError("Manual Session Disconnect Free error.");
                }
                else
                {
                    logger.LogError("Manual Session Disconnect End error.");
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task<SessionResponseModel> SendCreateSession(CreateSessionRequestModel request)
    {
        // session can only be created if has no owner and no session exists
        if (! await ownership.IsNone())
            throw new HubException("Manual Session Create error - cannot be created, since another user owns one.");
        
        var command = new CreateSessionCommand(
            HandlingMode.Manual,
            request.MappingEnabled,
            request.InputMapRef,
            request.OutputMapRef,
            request.OutputMapName);
        
        var result = await mediator.Send(command);
        if (!result.IsSuccess) throw new HubException("Manual Session Create error.");
        
        var isReserved = await ownership.Reserve(result.Value, Context.ConnectionId);
        if (!isReserved) throw new HubException("Manual Session Create error - could not be reserved.");
        
        var resultEntity = await mediator.Send(new GetSessionQuery(result.Value));
        if (!resultEntity.IsSuccess) throw new HubException("Manual Session Create error - could not get session response data");

        return SessionResponseModel.ToModel(resultEntity.Value);
    }
    
    public async Task<SessionResponseModel> SendStartSession()
    {
        if (! await ownership.IsReservedByMe(Context.ConnectionId))
            throw new HubException("Manual Session Start error - session could not be started, it is owned by somebody else or it has not been yet created.");

        var sessionId = await ownership.GetSessionId();
        if (sessionId.HasValue)
        {
            var result = await mediator.Send(new StartSessionCommand(sessionId.Value));
            if (!result.IsSuccess) throw new HubException("Manual Session Start error.");
            
            var resultEntity = await mediator.Send(new GetSessionQuery(sessionId.Value));
            if (!resultEntity.IsSuccess) throw new HubException("Manual Session Start error - could not get session response data");
            
            return SessionResponseModel.ToModel(resultEntity.Value);
        }

        throw new HubException("Manual Session Start error - current session not found.");
    }

    public async Task<SessionResponseModel> SendEndSession()
    {
        if (! await ownership.IsReservedByMe(Context.ConnectionId))
            throw new HubException("Manual Session End error - session could not be ended, it is owned by somebody else or it has not been yet created.");

        var sessionId = await ownership.GetSessionId();
        if (sessionId.HasValue)
        {
            var result = await mediator.Send(new EndSessionCommand(sessionId.Value));
            if (!result.IsSuccess) throw new HubException("Manual Session End error.");
            
            var isFree = await ownership.Free(Context.ConnectionId);
            if (!isFree) throw new HubException("Manual Session End error - could not free session.");
            
            var resultEntity = await mediator.Send(new GetSessionQuery(sessionId.Value));
            if (!resultEntity.IsSuccess) throw new HubException("Manual Session End error - could not get session response data");
                
            return SessionResponseModel.ToModel(resultEntity.Value);
        }
        
        throw new HubException("Manual Session End error - current session not found.");
    }
    
    public async Task SendJoy(JoyDto joy)
    {
        if (await ownership.IsReservedByMe(Context.ConnectionId)) await messageChannel.SendJoy(joy);
    }
}