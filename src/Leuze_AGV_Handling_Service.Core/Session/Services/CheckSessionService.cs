using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Checks session`s underlying process. Returns true if process is running, false otherwise.
/// </summary>
/// <param name="repository"></param>
/// <param name="processMonitorService"></param>
/// <param name="logger"></param>
public class CheckSessionService(
    IRepository<SessionAggregate.Session> repository,
    IProcessMonitorService processMonitorService,
    ILogger<CheckSessionService> logger
) : ICheckSessionService
{

    public async Task<Result<bool>> CheckSession(int sessionId)
    {
        logger.LogInformation($"Checking Session {sessionId}...");
        
        var spec = new SessionByIdWithActionsAndProcessesWithCommandsSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();
        
        var checkOk = await aggregate.CheckAsync(processMonitorService);

        await repository.UpdateAsync(aggregate);
        await repository.SaveChangesAsync();
        
        logger.LogInformation($"...checked Session {sessionId} with result {checkOk}");

        return Result.Success(checkOk);
    }

}