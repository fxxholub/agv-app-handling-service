using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;

namespace Handling_Service.UseCases.Messaging.CQRS.GetMap;

public record GetMapCommand(string connectionId) : Ardalis.SharedKernel.ICommand<Result<OccupancyGrid>>;