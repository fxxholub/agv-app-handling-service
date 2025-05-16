using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;

namespace Handling_Service.UseCases.Messaging.CQRS.LoadMap;

public record LoadMapCommand(string connectionId) : Ardalis.SharedKernel.ICommand<Result<OccupancyGrid>>;