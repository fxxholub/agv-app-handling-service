using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using MediatR;

namespace Handling_Service.UseCases.Messaging.Requests;

public record GetMapRequest() : IRequest<Result<OccupancyGrid>>;