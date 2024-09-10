using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Deletes session entity
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
        logger.LogInformation("Deleting Session {sessionId}", sessionId);
        
        // get session object from repository
        SessionAggregate.Session? aggregateToDelete = await repository.GetByIdAsync(sessionId);
        if (aggregateToDelete == null) return Result.NotFound();
        
        // delete the object from repository
        await repository.DeleteAsync(aggregateToDelete);
        return Result.Success();
    }

}