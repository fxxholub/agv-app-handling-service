using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Requests;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class GetMapHandler(IClientNode clientNode) : IRequestHandler<GetMapRequest, Result<OccupancyGrid>>
{
    public async Task<Result<OccupancyGrid>> Handle(GetMapRequest request, CancellationToken cancellationToken)
    {
        return await clientNode.GetMap();
    }
}