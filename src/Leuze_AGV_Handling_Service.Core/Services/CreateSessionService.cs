using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

public class CreateSessionService(
    IRepository<Session> sessionRepository,
    IStartSessionService startSessionService,
    IProcessProviderService processProviderService,
    ILogger<CreateSessionService> logger
) : ICreateSessionService
{
    public async Task<Session> CreateSession(
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
            // var createdProcess = await processRepository.AddAsync(process);
            createdSession.AddProcess(process);
        }

        await sessionRepository.UpdateAsync(createdSession);
        
        // start the session after creation
        await startSessionService.StartSession(createdSession.Id);
        
        logger.LogInformation("Created Session {sessionId}", createdSession.Id);
        return createdSession;
    }
}