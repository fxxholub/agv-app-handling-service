using System.Text.Unicode;
using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Models.Session;
using Handling_Service.Infrastructure.SignalR.Utils;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Messaging.CQRS.PublishCmdVel;
using Handling_Service.UseCases.Messaging.CQRS.PublishPath;
using Handling_Service.UseCases.Messaging.DTOs;
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
using IPublisher = Handling_Service.Infrastructure.SignalR.Interfaces.IPublisher;

namespace Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Handling Hub
/// </summary>
[SignalRHub(path: "/api/v1/handling/signalr")]
public class HandlingHub(
    IMediator mediator,
    ILogger<HandlingHub> logger
    ) : Hub<IHandlingHub>, IPublisher
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
    
    // Session Actions ////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task StartSession(HandlingMode handlingMode)
    {
        var result = await mediator.Send(new StartSessionCommand(Context.ConnectionId, handlingMode));
        
        if (result.IsSuccess)
        {
            var agvMode = "";
            if (handlingMode == HandlingMode.Autonomous)
            {
                agvMode = "automatic";
            }
            if (handlingMode == HandlingMode.Automatic)
            {
                agvMode = "automatic";
            }
            else if (handlingMode == HandlingMode.Manual)
            {
                agvMode = "manual";
            }
            await mediator.Publish(new AgvMode(agvMode));
        }
        
        ResultChecker<bool>.Check(result);
    }

    public async Task EndSession()
    {
        var result = await mediator.Send(new EndSessionCommand(Context.ConnectionId));
        
        ResultChecker<bool>.Check(result);
    }
    
    public async Task LeaveSession()
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        
        ResultChecker<bool>.Check(result);
    }
    
    // ROS Messages ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task PublishCmdVel(float x, float y, float w)
    {
        await mediator.Send(new PublishCmdVelCommand(Context.ConnectionId, new CmdVelDto(x, y, w)));
    }
    
    public async Task PublishPath(string path)
    {
        await mediator.Send(new PublishPathCommand(Context.ConnectionId, path));
    }
}