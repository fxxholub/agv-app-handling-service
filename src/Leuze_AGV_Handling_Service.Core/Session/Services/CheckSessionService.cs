using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

/// <summary>
/// Checks session`s underlying process, notifies the system about bad check
/// </summary>
/// <param name="repository"></param>
/// <param name="mediator"></param>
/// <param name="processHandlerService"></param>
/// <param name="logger"></param>
public class CheckSessionService(
    IRepository<Session> repository,
    IProcessHandlerService processHandlerService,
    ILogger<CheckSessionService> logger
) : ICheckSessionService
{

    public async Task<Result<bool>> CheckSession(int sessionId)
    {
        logger.LogInformation("Checking Session {sessionId}", sessionId);
        
        // get the session aggregate by id
        Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();
        
        var checkOk = await aggregate.CheckAsync(processHandlerService);
        
        // // notify system about bad check
        // if (!checkOk)
        // {
        //     logger.LogWarning("Session Faulty {sessionId}", sessionId);
        //     var domainEvent = new SessionFaultyEvent(sessionId);
        //     await mediator.Publish(domainEvent);
        // }

        return Result.Success(checkOk);
    }

}