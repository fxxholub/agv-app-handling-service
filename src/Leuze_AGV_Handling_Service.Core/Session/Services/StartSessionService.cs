using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Events;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Starts session with its underlying processes.
/// </summary>
/// <param name="repository"></param>
/// <param name="mediator"></param>
/// <param name="processHandlerService"></param>
/// <param name="logger"></param>
public class StartSessionService(
    IRepository<SessionAggregate.Session> repository,
    IMediator mediator,
    IProcessHandlerService processHandlerService,
    ILogger<StartSessionService> logger
) : IStartSessionService
{

    public async Task<Result> StartSession(int sessionId)
    {
        logger.LogInformation("Starting Session {sessionId}", sessionId);
        
        
        SessionAggregate.Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();

        await aggregate.StartAsync(processHandlerService);
        
        // notify system about start
        var domainEvent = new SessionStartedEvent(sessionId, aggregate.HandlingMode);
        await mediator.Publish(domainEvent);

        await repository.UpdateAsync(aggregate);

        if (aggregate.State != SessionState.Started)
        {
            logger.LogError("Session not Started");
            return Result.Error();
        }

        return Result.Success();
    }

}