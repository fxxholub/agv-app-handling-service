using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Creates session entity.
/// </summary>
/// <param name="sessionRepository"></param>
/// <param name="processProviderService"></param>
/// <param name="logger"></param>
public class CreateSessionService(
    IRepository<SessionAggregate.Session> sessionRepository,
    IProcessProviderService processProviderService,
    ILogger<CreateSessionService> logger
) : ICreateSessionService
{
    public async Task<Result<int>> CreateSession(
        HandlingMode handlingMode,
        bool mappingEnabled,
        string? inputMapRef,
        string? outputMapRef,
        string? outputMapName
        )
    {
        logger.LogInformation("Creating Session...");
        
        // create session object
        var newSession = new SessionAggregate.Session(
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
        
        logger.LogInformation(createdSession.Processes.Count().ToString());
        
        await sessionRepository.UpdateAsync(createdSession);
        await sessionRepository.SaveChangesAsync();
        
        // // get the session from repository to check if added sucessfully
        // SessionAggregate.Session? aggregate = await sessionRepository.GetByIdAsync(createdSession.Id);
        // if (aggregate is null) return Result.NotFound();
        
        logger.LogInformation($"...created Session {createdSession.Id}.");

        return Result.Success(createdSession.Id);
    }
}
