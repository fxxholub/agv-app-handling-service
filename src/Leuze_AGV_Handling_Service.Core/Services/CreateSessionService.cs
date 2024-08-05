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

        foreach (var process in processProviderService.GetProcesses(handlingMode))
        {
            // newSession.AddProcess(process);
        }
        
        var createdItem = await sessionRepository.AddAsync(newSession);
        
        // start the session after creation
        await startSessionService.StartSession(createdItem.Id);
        
        logger.LogInformation("Created Session {sessionId}", createdItem.Id);
        return createdItem;
    }
}