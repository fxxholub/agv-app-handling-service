using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Microsoft.Extensions.Logging;

namespace Handling_Service.Core.Session.Services;

/// <summary>
/// Creates session entity and populates session with its processes. Stores the session in repository.
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
    /// <summary>
    /// Creates Session entity and stores it into repository. Creation involves load of processes in it.
    /// </summary>
    /// <param name="handlingMode"></param>
    /// <param name="lifespan"></param>
    /// <param name="mapping"></param>
    /// <returns>
    /// - Task Result.Success(): returns id of the created session.
    /// </returns>
    public async Task<Result<int>> CreateSession(HandlingMode handlingMode, Lifespan lifespan, bool mapping)
    {
        logger.LogInformation("Creating Session.");
        
        // create session object
        var newSession = new SessionAggregate.Session(handlingMode, lifespan, mapping);

        // add session object to repository
        var createdSession = await sessionRepository.AddAsync(newSession);
        
        // add primary processes to session
        foreach (var process in processProviderService.GetProcesses(handlingMode))
        {
            createdSession.AddProcess(process);
        }
        // add secondary processes to session (mapping, if selected)
        if (createdSession.Mapping)
        {
            foreach (var process in processProviderService.GetProcesses(HandlingMode.Mapping))
            {
                createdSession.AddProcess(process);
            }
        }
        
        // commit to the repository
        await sessionRepository.UpdateAsync(createdSession);
        await sessionRepository.SaveChangesAsync();
        
        logger.LogInformation($"Created Session {createdSession.Id}.");

        return Result.Success(createdSession.Id);
    }
}
