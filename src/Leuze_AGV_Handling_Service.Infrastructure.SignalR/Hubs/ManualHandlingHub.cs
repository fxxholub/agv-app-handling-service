using System.Text.Unicode;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models.Session;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Utils;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.End;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Leave;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Start;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Delete;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Manual handling mode Hub
/// </summary>
[SignalRHub(path: "/api/v1/handling/signalr/manual")]
public class ManualHandlingHub(
    IMediator mediator,
    ILogger<ManualHandlingHub> logger
    ) : Hub<IManualHandlingHub>, IManualPublisher
{
    // HUB ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        if (!result.IsSuccess)
            logger.LogWarning($"{result.Status}: {result.Errors}.");

        await base.OnDisconnectedAsync(exception);
    }
    
    // Session CRUD ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task<SessionResponseModel> GetSession(int sessionId)
    {
        var result = await mediator.Send(new GetSessionQuery(sessionId));

        ResultChecker<SessionDto>.Check(result);
        
        return SessionResponseModel.ToModel(result.Value);
    }
    
    public async Task<SessionResponseModel> CreateSession()
    {
        var createResult = await mediator.Send(new CreateSessionCommand(
            HandlingMode.Manual, Lifespan.Exclusive));

        ResultChecker<int>.Check(createResult);
        
        var resultEntity = await mediator.Send(new GetSessionQuery(createResult.Value));
        
        ResultChecker<SessionDto>.Check(resultEntity);

        return SessionResponseModel.ToModel(resultEntity.Value);
    }
    
    public async Task DeleteSession(int sessionId)
    {
        var result = await mediator.Send(new DeleteSessionCommand(sessionId));

        ResultChecker<int>.Check(result);
    }
    
    // Session Actions ////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task StartSession(int sessionId)
    {
        var result = await mediator.Send(new StartSessionCommand(sessionId, Context.ConnectionId, HandlingMode.Manual));
        
        ResultChecker<bool>.Check(result);
    }

    public async Task EndSession(int sessionId)
    {
        var result = await mediator.Send(new EndSessionCommand(sessionId, Context.ConnectionId));
        
        ResultChecker<bool>.Check(result);
    }
    
    public async Task LeaveSession(int sessionId)
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        
        ResultChecker<bool>.Check(result);
    }
    
    // ROS Messages ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task PublishCmdVel(float x, float y, float w)
    {
        if (mediator.Send(new IsCurrentConnectionQuery(Context.ConnectionId)).Result.Value)
            await mediator.Publish(new CmdVel(x, y, w));
    }
}