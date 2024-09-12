using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Starts session with its underlying processes.
/// </summary>
/// <param name="repository"></param>
/// <param name="processMonitorService"></param>
/// <param name="logger"></param>
public class StartSessionService(
    IRepository<SessionAggregate.Session> repository,
    IProcessMonitorService processMonitorService,
    ILogger<StartSessionService> logger
) : IStartSessionService
{

    public async Task<Result> StartSession(int sessionId)
    {
        logger.LogInformation($"Starting Session {sessionId}...");
        
        // this doesnt work (gets the session without processes) - dont know why
        // SessionAggregate.Session? aggregate = await repository.GetByIdAsync(sessionId);
        // this works - dont know why
        var spec = new SessionByIdWithProcessesSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();

        logger.LogInformation(aggregate.Processes.Count().ToString());
        
        await aggregate.StartAsync(processMonitorService);

        await repository.UpdateAsync(aggregate);
        await repository.SaveChangesAsync();

        if (aggregate.State != SessionState.Started)
        {
            logger.LogInformation($"...start of Session {sessionId} err.");
            return Result.Error();
        }
        
        logger.LogInformation($"...started Session {sessionId}.");

        return Result.Success();
    }

}