using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
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
        
        
        SessionAggregate.Session? aggregate = await repository.GetByIdAsync(sessionId);
        if (aggregate == null) return Result.NotFound();

        await aggregate.StartAsync(processMonitorService);

        await repository.UpdateAsync(aggregate);

        if (aggregate.State != SessionState.Started)
        {
            logger.LogInformation($"...start of Session {sessionId} err.");
            return Result.Error();
        }
        
        logger.LogInformation($"...started Session {sessionId}.");

        return Result.Success();
    }

}