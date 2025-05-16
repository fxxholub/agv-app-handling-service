using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav2;
using Handling_Service.Core.Ros2.Interfaces.Std;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.UseCases.Messaging.Requests;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishGoalPose;

public class PublishGoalPoseHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<PublishGoalPoseCommand, Result>
{
    public async Task<Result> Handle(PublishGoalPoseCommand request, CancellationToken cancellationToken)
    {
        var isCurrentConnection = sessionExecutor.IsCurrentConnection(request.connectionId).Result.Value;
    
        if (
            isCurrentConnection
        )
        {
            var poseStamped = new PoseStamped(
                new Header{ FrameId = "map"},
                request.pose
            );
            await mediator.Publish(new PublishGoalPoseTopic(poseStamped), cancellationToken);
        }
        return new Result();
    }
}