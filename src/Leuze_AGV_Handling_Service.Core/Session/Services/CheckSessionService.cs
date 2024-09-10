using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Checks session`s underlying process, notifies the system about bad check
/// </summary>
/// <param name="repository"></param>
/// <param name="mediator"></param>
/// <param name="processHandlerService"></param>
/// <param name="logger"></param>
public class CheckSessionService(
    IRepository<SessionAggregate.Session> repository,
    IProcessHandlerService processHandlerService,
    ILogger<CheckSessionService> logger
) : ICheckSessionService
{

    public async Task<Result<bool>> CheckSession(int sessionId)
    {
        logger.LogInformation("Checking Session {sessionId}", sessionId);
        
        // get the session aggregate by id
        SessionAggregate.Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();
        
        var checkOk = await aggregate.CheckAsync(processHandlerService);

        return Result.Success(checkOk);
    }

}