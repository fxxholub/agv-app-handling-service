using Ardalis.Result;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishDriving;

/// <summary>
/// Publishes driving flag (resolves if robot moves or not in automatic mode) ROS message
/// </summary>
/// <param name="driving"></param>
public record PublishDrivingCommand(string connectionId, bool driving) : Ardalis.SharedKernel.ICommand<Result>;
