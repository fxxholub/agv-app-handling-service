using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Events;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.Core.Session.SessionAggregate.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Handling_Service.Core.Session.Services;

/// <summary>
/// Starts session with its underlying processes.
/// </summary>
/// <param name="repository"></param>
/// <param name="processMonitorFactory"></param>
/// <param name="logger"></param>
public class StartSessionService(
    IRepository<SessionAggregate.Session> repository,
    IProcessMonitorServiceFactory processMonitorFactory,
    ILogger<StartSessionService> logger,
    IMediator mediator,
    IEndSessionService endService
) : IStartSessionService
{
    /// <summary>
    /// Starts the session, effectively executing its processes and bringing it into the running (Started) state.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>
    /// - Task Result.Success(): on success.
    /// - Task Result.NotFound(): if session by sessionId is not found.
    /// - Task Result.Error(): if session did not start successfully.
    /// </returns>
    public async Task<Result> StartSession(int sessionId)
    {
        logger.LogInformation($"Starting Session {sessionId}.");
        
        // load the session entity from repository
        var spec = new SessionByIdWithActionsAndProcessesWithCommandsSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();
        
        // execute the starting operation
        await aggregate.StartAsync(processMonitorFactory);

        // commit the changes in the repository
        await repository.UpdateAsync(aggregate);
        await repository.SaveChangesAsync();

        // check the start operation outcome
        if (aggregate.State != SessionState.Started)
        {
            logger.LogWarning($"Start of Session {sessionId} errored. Some process/es did not start. Ending Session.");
            await endService.EndSession(sessionId);
            return Result.Error(new ErrorList(["Start of Session {sessionId} errored. Some process/es did not start. Ending Session."]));
        }
        
        // raise event indicating the session start
        var domainEvent = new SessionStartedEvent(sessionId);
        await mediator.Publish(domainEvent);
        
        logger.LogInformation($"Started Session {sessionId}.");

        return Result.Success();
    }

}