using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav2;
using Handling_Service.Core.Ros2.Interfaces.Std;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.UseCases.Messaging.Requests;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishInitialPose;

public class PublishInitialPoseHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<PublishInitialPoseCommand, Result>
{
    public async Task<Result> Handle(PublishInitialPoseCommand request, CancellationToken cancellationToken)
    {
        var isCurrentConnection = sessionExecutor.IsCurrentConnection(request.connectionId).Result.Value;
    
        if (
            isCurrentConnection
        )
        {
            var poseCovStamped = new PoseWithCovarianceStamped(
                new Header{ FrameId = "map"},
                new PoseWithCovariance{ Pose = request.pose }
            );
            await mediator.Publish(new PublishInitialPoseTopic(poseCovStamped), cancellationToken);
        }
        return new Result();
    }
}