using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Messaging.Requests;
using MediatR;

namespace Handling_Service.UseCases.Messaging.CQRS.GetMap;

public class GetMapHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<GetMapCommand, Result<OccupancyGrid>>
{
    public async Task<Result<OccupancyGrid>> Handle(GetMapCommand request, CancellationToken cancellationToken)
    {
        var isCurrentConnection = sessionExecutor.IsCurrentConnection(request.connectionId).Result.Value;
    
        if (
            isCurrentConnection
        )
        {
            var response = await mediator.Send(new GetMapRequest(), cancellationToken);
            return response;
        }
        return Result.Unauthorized();
    }
}