using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

/// <summary>
/// Deletes session entity, but ends the session underlying processes beforehand.
/// </summary>
/// <param name="repository"></param>
/// <param name="endSessionService"></param>
/// <param name="logger"></param>
public class DeleteSessionService(
    IRepository<Session> repository,
    IEndSessionService endSessionService,
    ILogger<DeleteSessionService> logger
) : IDeleteSessionService
{

    public async Task<Result> DeleteSession(int sessionId)
    {
        logger.LogInformation("Deleting Session {sessionId}", sessionId);
        Session? aggregateToDelete = await repository.GetByIdAsync(sessionId);
        if (aggregateToDelete == null) return Result.NotFound();

        // end session first, before it is deleted
        await endSessionService.EndSession(sessionId);
        
        await repository.DeleteAsync(aggregateToDelete);
        return Result.Success();
    }

}