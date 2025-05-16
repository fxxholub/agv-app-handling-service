using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Ros2.Interfaces.Nav2;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.UseCases.Messaging.Requests;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishAndSaveMap;

public class PublishAndSaveMapHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<PublishAndSaveMapCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(PublishAndSaveMapCommand request, CancellationToken cancellationToken)
    {
        var isCurrentConnection = sessionExecutor.IsCurrentConnection(request.connectionId).Result.Value;
    
        if (
            isCurrentConnection
        )
        {
            await mediator.Publish(new PublishMapTopic(request.map), cancellationToken);
            var saveMapOptions = new SaveMapServiceRequest
            {
                MapTopic = "map", 
                MapUrl = "/ros2_ws/src/diff_robot_gazebo/maps/map", 
                ImageFormat = "pgm", 
                MapMode = "trinary", 
                FreeThresh = 0.25f, 
                OccupiedThresh = 0.65f
            };
            var response = await mediator.Send(new SaveMapRequest(saveMapOptions), cancellationToken);
            var loadMapOptions = new LoadMapServiceRequest
            {
                MapUrl = "/ros2_ws/src/diff_robot_gazebo/maps/map.yaml"
            };
            await mediator.Send(new LoadMapRequest(loadMapOptions), cancellationToken);
            return response;
        }
        return Result.Unauthorized();
    }
}