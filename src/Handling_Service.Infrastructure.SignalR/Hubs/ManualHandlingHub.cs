using System.Text.Unicode;
using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Models.Session;
using Handling_Service.Infrastructure.SignalR.Utils;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Messaging.Topics;
using Handling_Service.UseCases.Session.CQRS.CRUD.Create;
using Handling_Service.UseCases.Session.CQRS.Actions.End;
using Handling_Service.UseCases.Session.CQRS.Actions.Leave;
using Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using Handling_Service.UseCases.Session.CQRS.Actions.Start;
using Handling_Service.UseCases.Session.CQRS.CRUD.Delete;
using Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;
using Handling_Service.UseCases.Session.DTOs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace Handling_Service.Infrastructure.SignalR.Hubs;

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