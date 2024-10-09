using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Delete;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.End;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.GetCurrent;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Start;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Hub for all manual messages.
/// </summary>
/// <param name="messageChannel"></param>
// [ApiVersion(1)]
// [SignalRHub(path: "api/v{v:apiVersion}/signalr/handling-hub")]
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
        await ownership.Free(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendCreateSession(CreateSessionRequestModel request)
    {
        // session can only be created if has no owner and no session exists
        if (! await ownership.IsNone())
        {
            logger.LogInformation("Manual Session cannot be created, since another user owns one.");
            return;
        }
        
        var command = new CreateSessionCommand(
            HandlingMode.Manual,
            request.MappingEnabled,
            request.InputMapRef,
            request.OutputMapRef,
            request.OutputMapName);
        
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
        {
            logger.LogInformation("Manual Session creation errored.");
            return;
        }
        // reserve session ownership
        var isReserved = await ownership.Reserve(result.Value, Context.ConnectionId);

        if (!isReserved)
        {
            logger.LogInformation("Manual Session could not be reserved.");
            return;
        }
        
        var resultEntity = await mediator.Send(new GetSessionQuery(result.Value));

        if (resultEntity.IsSuccess)
        {
            await Clients.All.ReceiveSession(SessionResponseModel.ToModel(resultEntity.Value));
        }
        else
        {
            // TODO: send error
        }
    }
    
    public async Task SendStartSession()
    {
        if (! await ownership.IsReservedByMe(Context.ConnectionId))
        {
            logger.LogInformation("Manual Session could not be started, it is owned by somebody else or it has not been yet created.");
            return;
        }

        var sessionId = await ownership.GetSessionId();
        if (sessionId.HasValue)
        {
            var result = await mediator.Send(new StartSessionCommand(sessionId.Value));

            if (!result.IsSuccess)
            {
                // TODO
                logger.LogInformation("Manual Session start errored.");
                return;
            }
            var resultEntity = await mediator.Send(new GetSessionQuery(sessionId.Value));
            
            if (resultEntity.IsSuccess)
            {
                await Clients.All.ReceiveSession(SessionResponseModel.ToModel(resultEntity.Value));
            }
            else
            {
                // TODO: send error
            }
        }
    }

    public async Task SendEndSession()
    {
        if (! await ownership.IsReservedByMe(Context.ConnectionId))
        {
            // TODO: notify user
            logger.LogInformation("Manual Session could not be ended, it is owned by somebody else or it has not been yet created.");
            return;
        }

        var sessionId = await ownership.GetSessionId();
        if (sessionId.HasValue)
        {
            var result = await mediator.Send(new EndSessionCommand(sessionId.Value));

            if (!result.IsSuccess)
            {
                logger.LogInformation("Manual Session end errored.");
                return;
            }
            
            var isFree = await ownership.Free(Context.ConnectionId);

            if (!isFree)
            {
                logger.LogInformation("Manual Session free errored.");
                return;
            }
            
            var resultEntity = await mediator.Send(new GetSessionQuery(sessionId.Value));
            
            if (resultEntity.IsSuccess)
            {
                await Clients.All.ReceiveSession(SessionResponseModel.ToModel(resultEntity.Value));
            }
            else
            {
                // TODO: send error
            }
        }
        
    }
    
    public async Task SendJoy(JoyDTO joy)
    {
        await messageChannel.SendJoy(joy);
    }
}