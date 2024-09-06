using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

/// <summary>
/// Ends session`s underlying processes.
/// </summary>
/// <param name="repository"></param>
/// <param name="mediator"></param>
/// <param name="processHandlerService"></param>
/// <param name="logger"></param>
public class EndSessionService(
    IRepository<Session> repository,
    IMediator mediator,
    IProcessHandlerService processHandlerService,
    ILogger<EndSessionService> logger
) : IEndSessionService
{

    public async Task<Result> EndSession(int sessionId)
    {
        logger.LogInformation("Ending Session {sessionId}", sessionId);
        
        
        Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();

        await aggregate.EndAsync(processHandlerService);
        
        // notify system about session end
        var domainEvent = new SessionEndedEvent(sessionId);
        await mediator.Publish(domainEvent);

        await repository.UpdateAsync(aggregate);

        return Result.Success();
    }

}