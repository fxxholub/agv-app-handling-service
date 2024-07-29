using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

public class DeleteSessionService(
    IRepository<Session> repository,
    IMediator mediator,
    ILogger<DeleteSessionService> logger
) : IDeleteSessionService
{

    public async Task<Result> DeleteSession(int sessionId)
    {
        logger.LogInformation("Deleting Session {sessionId}", sessionId);
        Session? aggregateToDelete = await repository.GetByIdAsync(sessionId);
        if (aggregateToDelete == null) return Result.NotFound();

        await repository.DeleteAsync(aggregateToDelete);
        var domainEvent = new SessionDeletedEvent(sessionId);
        await mediator.Publish(domainEvent);
        return Result.Success();
    }

}