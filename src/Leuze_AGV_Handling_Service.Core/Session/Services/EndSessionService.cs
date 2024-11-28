using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Events;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Ends session`s underlying processes.
/// </summary>
/// <param name="repository"></param>
/// <param name="processMonitorFactory"></param>
/// <param name="logger"></param>
public class EndSessionService(
    IRepository<SessionAggregate.Session> repository,
    IProcessMonitorServiceFactory processMonitorFactory,
    ILogger<EndSessionService> logger,
    IMediator mediator
) : IEndSessionService
{

    /// <summary>
    /// Ends the session, effectivelly killing its processes and bringing off the running (Started) state.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>
    /// - Task Result.Success(): on success.
    /// - Task Result.NotFound(): if session by sessionId is not found.
    /// </returns>
    public async Task<Result> EndSession(int sessionId)
    {
        logger.LogInformation($"Ending Session {sessionId}.");
        
        // load the entity from repository
        var spec = new SessionByIdWithActionsAndProcessesWithCommandsSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();

        // end it
        await aggregate.EndAsync(processMonitorFactory);

        // commit changes in the reopsitory
        await repository.UpdateAsync(aggregate);
        await repository.SaveChangesAsync();
        
        // raise event indicating the session end
        var domainEvent = new SessionEndedEvent(sessionId);
        await mediator.Publish(domainEvent);
        
        logger.LogInformation($"Ended Session {sessionId}.");

        return Result.Success();
    }

}