using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishAndSaveMap;

public record PublishAndSaveMapCommand(string connectionId, OccupancyGrid map) : Ardalis.SharedKernel.ICommand<Result<bool>>;