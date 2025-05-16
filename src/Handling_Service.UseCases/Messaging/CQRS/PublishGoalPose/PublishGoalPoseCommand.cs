using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishGoalPose;

public record PublishGoalPoseCommand(string connectionId, Pose pose) : Ardalis.SharedKernel.ICommand<Result>;