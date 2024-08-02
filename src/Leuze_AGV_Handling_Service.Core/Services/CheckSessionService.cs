using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

public class CheckSessionService(
    IRepository<Session> repository,
    IMediator mediator,
    IProcessHandlerService processHandlerService,
    ILogger<StartSessionService> logger
) : ICheckSessionService
{

    public async Task<Result> CheckSession(int sessionId)
    {
        logger.LogInformation("Checking Session {sessionId}", sessionId);
        
        Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();

        // notify system about bad check
        if (await aggregate.CheckAsync(processHandlerService))
        {
            logger.LogWarning("Session Faulty {sessionId}", sessionId);
            var domainEvent = new SessionFaultyEvent(sessionId);
            await mediator.Publish(domainEvent);
        }

        return Result.Success();
    }

}