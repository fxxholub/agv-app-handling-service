using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Deletes non active session entity from repository.
/// </summary>
/// <param name="repository"></param>
/// <param name="logger"></param>
public class DeleteSessionService(
    IRepository<SessionAggregate.Session> repository,
    ILogger<DeleteSessionService> logger
) : IDeleteSessionService
{
    
    /// <summary>
    /// Deletes session entity from repository. Only proceeds if the session is not in the running state.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>
    /// - Task Result.Success(): on success.
    /// - Task Result.NotFound(): if session by sessionId is not found.
    /// - Task Result.Conflict(): if session is running (active) meaning it is in Started state.
    /// </returns>
    public async Task<Result> DeleteSession(int sessionId)
    {
        logger.LogInformation($"Deleting Session {sessionId}.");
        
        // load the entity from repository
        var spec = new SessionByIdWithActionsAndProcessesWithCommandsSpec(sessionId);
        SessionAggregate.Session? aggregate = await repository.FirstOrDefaultAsync(spec);
        
        if (aggregate == null) return Result.NotFound();
        
        // do not delete running session
        if (aggregate.State == SessionState.Started)
        {
            logger.LogWarning($"Deletion failed: cannot delete Session which is actively running (Started).");
            return Result.Conflict();
        }
        
        // commit the delete in repository
        await repository.DeleteAsync(aggregate);
        await repository.SaveChangesAsync();
        
        logger.LogInformation($"Deleted Session {sessionId}.");
        
        return Result.Success();
    }

}