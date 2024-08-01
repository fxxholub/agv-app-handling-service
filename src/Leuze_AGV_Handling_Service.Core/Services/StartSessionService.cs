using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

public class StartSessionService(
    IRepository<Session> repository,
    IMediator mediator,
    ILogger<StartSessionService> logger
) : IStartSessionService
{

    public async Task<Result> StartSession(int sessionId)
    {
        logger.LogInformation("Starting Session {sessionId}", sessionId);
        
        
        Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();

        await aggregate.StartAsync();
        
        // notify system about start
        var domainEvent = new SessionStartedEvent(sessionId);
        await mediator.Publish(domainEvent);

        await repository.UpdateAsync(aggregate);

        return Result.Success();
    }

}