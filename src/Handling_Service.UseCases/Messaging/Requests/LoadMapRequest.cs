using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using MediatR;
using LoadMapServiceRequest = Handling_Service.Core.Ros2.Interfaces.Nav2.LoadMapServiceRequest;

namespace Handling_Service.UseCases.Messaging.Requests;

public record LoadMapRequest(LoadMapServiceRequest Data) : IRequest<Result<OccupancyGrid>>;