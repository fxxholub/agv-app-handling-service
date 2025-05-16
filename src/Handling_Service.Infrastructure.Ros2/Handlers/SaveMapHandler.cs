using Ardalis.Result;
using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.Requests;
using MediatR;

namespace Handling_Service.Infrastructure.Ros2.Handlers;

public class SaveMapHandler(IClientNode clientNode) : IRequestHandler<SaveMapRequest, Result<bool>>
{
    public async Task<Result<bool>> Handle(SaveMapRequest request, CancellationToken cancellationToken)
    {
        return await clientNode.SaveMap(request.Data);
    }
}