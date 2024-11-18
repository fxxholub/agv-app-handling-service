using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Deletes session entity.
/// </summary>
/// <param name="repository"></param>
/// <param name="logger"></param>
public class DeleteSessionService(
    IRepository<SessionAggregate.Session> repository,
    ILogger<DeleteSessionService> logger
) : IDeleteSessionService
{

    public async Task<Result> DeleteSession(int sessionId)
    {
        logger.LogInformation($"Deleting Session {sessionId}...");
        
        var spec = new SessionByIdWithActionsAndProcessesWithCommandsSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();
        
        // delete the object from repository
        if (aggregate.State == SessionState.Started)
        {
            logger.LogInformation($"...Could not delete Session which is actively running.");
            return Result.Conflict();
        }
        await repository.DeleteAsync(aggregate);
        await repository.SaveChangesAsync();
        
        logger.LogInformation($"...deleted Session {sessionId}.");
        
        return Result.Success();
    }

}