using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Core.Ros2.Interfaces.Std;
using Handling_Service.Infrastructure.SignalR.Utils;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Models.Ros2;
using Handling_Service.UseCases.Messaging.CQRS.GetMap;
using Handling_Service.UseCases.Messaging.CQRS.LoadMap;
using Handling_Service.UseCases.Messaging.CQRS.PublishAndSaveMap;
using Handling_Service.UseCases.Messaging.CQRS.PublishCmdVel;
using Handling_Service.UseCases.Messaging.CQRS.PublishDriving;
using Handling_Service.UseCases.Messaging.CQRS.PublishGoalPose;
using Handling_Service.UseCases.Messaging.CQRS.PublishInitialPose;
using Handling_Service.UseCases.Messaging.CQRS.PublishPath;
using Handling_Service.UseCases.Messaging.Topics;
using Handling_Service.UseCases.Session.CQRS.Actions.End;
using Handling_Service.UseCases.Session.CQRS.Actions.Leave;
using Handling_Service.UseCases.Session.CQRS.Actions.Start;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace Handling_Service.Infrastructure.SignalR.Hubs;

/// <summary>
/// Handling Hub
/// </summary>
[Authorize]
[SignalRHub(path: HandlingHub.HubPath)]
public class HandlingHub(
    IMediator mediator,
    ILogger<HandlingHub> logger
    ) : Hub<IHandlingHub>, IEndpointHub
{
    // HUB ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public const string HubPath = "/api/v1/handling/signalr";
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await mediator.Send(new LeaveSessionCommand(Context.ConnectionId));
        if (!result.IsSuccess)
            logger.LogWarning($"{result.Status}: {result.Errors}.");

        await base.OnDisconnectedAsync(exception);
    }
    
    // Session Actions ////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task StartSession(HandlingMode handlingMode, bool mapping)
    {
        var result = await mediator.Send(new StartSessionCommand(Context.ConnectionId, handlingMode, mapping));
        
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
            await mediator.Publish(new AgvModeTopic(agvMode));
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
    
    // Messaging ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    public async Task PublishCmdVel(float x, float y, float w)
    {
        await mediator.Send(new PublishCmdVelCommand(
            Context.ConnectionId,
            new Twist(new Vector3(x, y, 0), new Vector3(0, 0, w))));
    }
    
    public async Task PublishPath(string path)
    {
        await mediator.Send(new PublishPathCommand(Context.ConnectionId, path));
    }
    
    public async Task PublishDriving(bool driving)
    {
        await mediator.Send(new PublishDrivingCommand(Context.ConnectionId, driving));
    }

    public async Task<bool> PublishAndSaveMap(OccupancyGridModel map)
    {
        var response = await mediator.Send(new PublishAndSaveMapCommand(
            Context.ConnectionId,
            new OccupancyGrid
            {
                Header = new Header{ FrameId = "map" },
                Info = new MapMetaData
                {
                    Resolution = map.Resolution,
                    Width = map.Width,
                    Height = map.Height,
                    Origin = new Pose
                    {
                        Position = new Point(map.Origin.Position.X, map.Origin.Position.Y, map.Origin.Position.Z),
                        Orientation = new Quaternion(map.Origin.Orientation.X, map.Origin.Orientation.Y, map.Origin.Orientation.Z, map.Origin.Orientation.W)
                    },
                },
                Data = map.Data
            }
        ));
        
        ResultChecker<bool>.Check(response);
        
        return response.Value;
    }

    public async Task<OccupancyGridModel> LoadMap()
    {
        var response = await mediator.Send(new LoadMapCommand(Context.ConnectionId));
        
        ResultChecker<OccupancyGrid>.Check(response);
        
        return new OccupancyGridModel
        (
            response.Value.Info.Resolution,
            response.Value.Info.Width,
            response.Value.Info.Height,
            new PoseModel
            (
                new PointModel(
                    response.Value.Info.Origin.Position.X,
                    response.Value.Info.Origin.Position.Y,
                    response.Value.Info.Origin.Position.Z),
                new QuaternionModel(
                    response.Value.Info.Origin.Orientation.X,
                    response.Value.Info.Origin.Orientation.Y,
                    response.Value.Info.Origin.Orientation.Z,
                    response.Value.Info.Origin.Orientation.W)
            ),
            response.Value.Data
        );
    }
    
    public async Task<OccupancyGridModel> GetMap()
    {
        var response = await mediator.Send(new GetMapCommand(Context.ConnectionId));
        
        ResultChecker<OccupancyGrid>.Check(response);
        
        return new OccupancyGridModel
        (
            response.Value.Info.Resolution,
            response.Value.Info.Width,
            response.Value.Info.Height,
            new PoseModel
            (
                new PointModel(
                    response.Value.Info.Origin.Position.X,
                    response.Value.Info.Origin.Position.Y,
                    response.Value.Info.Origin.Position.Z),
                new QuaternionModel(
                    response.Value.Info.Origin.Orientation.X,
                    response.Value.Info.Origin.Orientation.Y,
                    response.Value.Info.Origin.Orientation.Z,
                    response.Value.Info.Origin.Orientation.W)
            ),
            response.Value.Data
        );
    }

    public async Task PublishInitialPose(PoseModel pose)
    {
        var pos = new Pose(
            new Point(pose.Position.X, pose.Position.Y, pose.Position.Z),
            new Quaternion(pose.Orientation.X, pose.Orientation.Y, pose.Orientation.Z, pose.Orientation.W));
        await mediator.Send(new PublishInitialPoseCommand(Context.ConnectionId, pos));
    }

    public async Task PublishGoalPose(PoseModel pose)
    {
        var pos = new Pose(
            new Point(pose.Position.X, pose.Position.Y, pose.Position.Z),
            new Quaternion(pose.Orientation.X, pose.Orientation.Y, pose.Orientation.Z, pose.Orientation.W));
        await mediator.Send(new PublishGoalPoseCommand(Context.ConnectionId, pos));
    }
}