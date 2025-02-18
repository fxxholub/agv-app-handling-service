using Ardalis.Result;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishPath;

/// <summary>
/// Publishes cmd vel ROS message
/// </summary>
/// <param name="path">linear axis X</param>
public record PublishPathCommand(string connectionId, string path) : Ardalis.SharedKernel.ICommand<Result>;
