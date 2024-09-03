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
/// <param name="startSessionService"></param>
/// <param name="processProviderService"></param>
/// <param name="logger"></param>
public class CreateSessionService(
    IRepository<Session> sessionRepository,
    IStartSessionService startSessionService,
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
        
        var newSession = new Session(
            handlingMode,
            mappingEnabled,
            inputMapRef,
            outputMapRef,
            outputMapName
            );

        var createdSession = await sessionRepository.AddAsync(newSession);
        
        foreach (var process in processProviderService.GetProcesses(handlingMode))
        {
            createdSession.AddProcess(process);
        }
        await sessionRepository.UpdateAsync(createdSession);
        
        // start the session after creation
        await startSessionService.StartSession(createdSession.Id);
        
        logger.LogInformation("Created Session {sessionId}", createdSession.Id);
        
        Session? aggregate = await sessionRepository.GetByIdAsync(createdSession.Id);
        if (aggregate == null) return Result.NotFound();
        return Result.Success(createdSession);
    }
}
