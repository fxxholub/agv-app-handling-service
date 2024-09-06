using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Services;

public class SessionManagerService(
    ICreateSessionService createSessionService,
    IDeleteSessionService deleteSessionService,
    IStartSessionService startSessionService,
    IEndSessionService endSessionService,
    ICheckSessionService checkSessionService
    
    ) : ISessionManagerService
{
    private int? _currentSessionId = null;

    public async Task<Result<Session>> CreateAndStartSession(
        HandlingMode handlingMode,
        bool mappingEnabled,
        string? inputMapRef,
        string? outputMapRef,
        string? outputMapName
        ) 
    {
        if (_currentSessionExists()) return Result.Conflict();
        
        // create the session
        var session = await createSessionService.CreateSession(
            handlingMode,
            mappingEnabled,
            inputMapRef,
            outputMapRef,
            outputMapName
        );
        
        if (!session.IsSuccess) return Result.Error();

        // start the session along with its processes
        var started = await startSessionService.StartSession(session.Value.Id);

        if (!started.IsSuccess) return Result.Error();
        
        // set the session as current
        _currentSessionId = session.Value.Id;
        
        return Result.Success(session);

    }

    public async Task<Result> EndAndDeleteSession(int sessionId)
    {
        if (_isCurrentSession(sessionId))
        {
            // if session is current session, first end it
            var ended = await endSessionService.EndSession(sessionId);
            if (!ended.IsSuccess) return Result.Error();
            
            // right after session ends, new session could be made
            _currentSessionId = null;
        }
        
        // finally, delete the session
        var deleted = await deleteSessionService.DeleteSession(sessionId);
        if (!deleted.IsSuccess) return Result.Error();

        return Result.Success();
    }

    public async Task<Result> EndSession(int sessionId)
    {
        if (_isCurrentSession(sessionId))
        {
            var ended = await endSessionService.EndSession(sessionId);
            if (!ended.IsSuccess) return Result.Error();
            
            // right after session ends, new session could be made
            _currentSessionId = null;

            return Result.Success();
        }

        return Result.Conflict();
    }

    public async Task<Result<bool>> CheckAndEndBadSession(int sessionId)
    {
        if (_isCurrentSession(sessionId))
        {
            var checkedResult = await checkSessionService.CheckSession(sessionId);
            if (!checkedResult.IsSuccess) return Result.Error();
            
            
            return Result.Success(checkedResult.Value);
        }

        return Result.Conflict();
    }
    
    private bool _currentSessionExists()
    {
        return _currentSessionId is not null;
    }
    
    private bool _isCurrentSession(int sessionId)
    {
        return _currentSessionId is not null;
    }
}