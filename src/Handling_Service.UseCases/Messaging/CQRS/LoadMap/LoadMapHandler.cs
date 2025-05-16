using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Core.Ros2.Interfaces.Nav2;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.UseCases.Messaging.Requests;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using LoadMapServiceRequest = Handling_Service.Core.Ros2.Interfaces.Nav2.LoadMapServiceRequest;

namespace Handling_Service.UseCases.Messaging.CQRS.LoadMap;

public class LoadMapHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<LoadMapCommand, Result<OccupancyGrid>>
{
    public async Task<Result<OccupancyGrid>> Handle(LoadMapCommand request, CancellationToken cancellationToken)
    {
        var isCurrentConnection = sessionExecutor.IsCurrentConnection(request.connectionId).Result.Value;
    
        if (
            isCurrentConnection
        )
        {
            var result = await mediator.Send(
                new LoadMapRequest(new LoadMapServiceRequest("/ros2_ws/src/diff_robot_gazebo/maps/map.yaml")));
            return result;
        }
        return Result.Unauthorized();
    }
}