using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

/// <summary>
/// Creates session entity, attempts to start the session by starting underlying processes.
/// </summary>
/// <param name="sessionRepository"></param>
/// <param name="processProviderService"></param>
/// <param name="logger"></param>
public class CreateSessionService(
    IRepository<Session> sessionRepository,
    IProcessProviderService processProviderService,
    ILogger<CreateSessionService> logger
) : ICreateSessionService
{
    public async Task<Result<Session>> CreateSession(
        HandlingMode handlingMode,
        bool mappingEnabled,
        string? inputMapRef,
        string? outputMapRef,
        string? outputMapName
        )
    {
        logger.LogInformation("Creating Session");
        
        // create session object
        var newSession = new Session(
            handlingMode,
            mappingEnabled,
            inputMapRef,
            outputMapRef,
            outputMapName
            );

        // add session object to repository
        var createdSession = await sessionRepository.AddAsync(newSession);
        
        // add processes to session
        foreach (var process in processProviderService.GetProcesses(handlingMode))
        {
            createdSession.AddProcess(process);
        }
        await sessionRepository.UpdateAsync(createdSession);
        
        logger.LogInformation("Created Session {sessionId}", createdSession.Id);
        
        // get the session from repository to check if added sucessfully
        Session? aggregate = await sessionRepository.GetByIdAsync(createdSession.Id);
        if (aggregate == null) return Result.NotFound();

        return Result.Success(createdSession);
    }
}
