using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Events;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Ends session`s underlying processes.
/// </summary>
/// <param name="repository"></param>
/// <param name="mediator"></param>
/// <param name="processHandlerService"></param>
/// <param name="logger"></param>
public class EndSessionService(
    IRepository<SessionAggregate.Session> repository,
    IMediator mediator,
    IProcessHandlerService processHandlerService,
    ILogger<EndSessionService> logger
) : IEndSessionService
{

    public async Task<Result> EndSession(int sessionId)
    {
        logger.LogInformation("Ending Session {sessionId}", sessionId);
        
        
        SessionAggregate.Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();

        await aggregate.EndAsync(processHandlerService);
        
        // notify system about session end
        var domainEvent = new SessionEndedEvent(sessionId, aggregate.HandlingMode);
        await mediator.Publish(domainEvent);

        await repository.UpdateAsync(aggregate);

        return Result.Success();
    }

}