using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Check Session Service allows to one-time check Session's processes.
///
/// </summary>
/// <param name="repository"></param>
/// <param name="processMonitorFactory"></param>
/// <param name="logger"></param>
public class CheckSessionService(
    IRepository<SessionAggregate.Session> repository,
    IProcessMonitorServiceFactory processMonitorFactory,
    ILogger<CheckSessionService> logger
) : ICheckSessionService
{

    /// <summary>
    /// Checks every Session's process condition.
    ///
    /// If process is running, returns true.
    /// If process is not running, returns false.
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>
    /// - Task Result.Success(): true if check ok, false otherwise
    /// - Task Result.NotFound(): if session by sessionId is not found
    /// </returns>
    public async Task<Result<bool>> CheckSession(int sessionId)
    {
        logger.LogInformation($"Checking Session {sessionId}.");
        
        // retrieve the session object from repository
        var spec = new SessionByIdWithActionsAndProcessesWithCommandsSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();
        
        // procees with the check operation
        var checkOk = await aggregate.CheckAsync(processMonitorFactory);

        // commit changes (if any) to the repository
        await repository.UpdateAsync(aggregate);
        await repository.SaveChangesAsync();
        
        logger.LogInformation($"Checked Session {sessionId} with result {checkOk}.");

        return Result.Success(checkOk);
    }

}