using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Geometry;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishCmdVel;

/// <summary>
/// Publishes cmd vel ROS message
/// </summary>
/// <param name="X">linear axis X</param>
/// <param name="Y">linear axis Y</param>
/// <param name="W">angular axis Z</param>
public record PublishCmdVelCommand(string connectionId, Twist data) : Ardalis.SharedKernel.ICommand<Result>;
