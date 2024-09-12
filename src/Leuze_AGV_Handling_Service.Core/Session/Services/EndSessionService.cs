using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Ends session`s underlying processes.
/// </summary>
/// <param name="repository"></param>
/// <param name="processMonitorService"></param>
/// <param name="logger"></param>
public class EndSessionService(
    IRepository<SessionAggregate.Session> repository,
    IProcessMonitorService processMonitorService,
    ILogger<EndSessionService> logger
) : IEndSessionService
{

    public async Task<Result> EndSession(int sessionId)
    {
        logger.LogInformation($"Ending Session {sessionId}...");
        
        var spec = new SessionByIdWithProcessesWithCommandsSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();

        await aggregate.EndAsync(processMonitorService);

        await repository.UpdateAsync(aggregate);
        await repository.SaveChangesAsync();
        
        logger.LogInformation($"...ended Session {sessionId}.");

        return Result.Success();
    }

}