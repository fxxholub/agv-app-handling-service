using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Models.Ros2;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.SignalR.Handlers;

public class RobotPoseHandler(IPushHub pushHub) : INotificationHandler<RobotPoseTopic>
{
    public async Task Handle(RobotPoseTopic message, CancellationToken cancellationToken)
    {

        var model = new PoseModel
        (
            new PointModel(
                message.data.Pose.Position.X,
                message.data.Pose.Position.Y,
                message.data.Pose.Position.Z),
            new QuaternionModel(
                message.data.Pose.Orientation.X,
                message.data.Pose.Orientation.Y,
                message.data.Pose.Orientation.Z,
                message.data.Pose.Orientation.W)
        );
        await pushHub.SubscribeRobotPose(model);
    }
}