using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Executor service handles Session's "singletonity" and ownership.
/// 
/// - Only one actively running session exists. Other sessions are inactive and stored only as records in repository.
/// - Only one connection can "own" ("reserve") the session.
/// </summary>
public class SessionExecutorService(
    IServiceProvider serviceProvider
    ) : ISessionExecutorService
{
    private int? _currentSessionId;
    private string? _currentConnectionId;

    /// <summary>
    /// Start session and reserving operation. Behavior based on Session's Lifespan.
    ///
    /// Based on Session's Lifespan, the execution will:
    /// - Exlusive: If session not active, start it. 
    /// - Extended: End the old extended session (old owner's session) and start a new one.
    ///
    /// Authorization based on connectionId:
    /// Only executes if no connection has reservation or when the input connectionId has reservation.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="connectionId"></param>
    /// <returns>
    /// - Task Result.Success()
    /// - Task Result.Unauthorized()
    /// - Task Result.Conflict()
    /// - Task Result.Error()
    /// </returns>
    public async Task<Result> StartSessionAndReserveConnection(int sessionId, string connectionId)
    {
        // authorization
        // start only if no connection active or connection my
        if (!(!IsCurrentConnectionActive() || IsCurrentConnection(connectionId).Result.Value))
            return Result.Unauthorized();
        
        // create scope for a call of this singleton method
        using var scope = serviceProvider.CreateScope();
        
        // retrieve repository
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<SessionAggregate.Session>>();
        
        // retrieve session entity
        var session = await repository.FirstOrDefaultAsync(new SessionByIdSpec(sessionId));
        if (session is null) return Result.Error();
        
        // retrieve starting service
        var startService = scope.ServiceProvider.GetRequiredService<IStartSessionService>();

        // lifespan rules
        switch (session.Lifespan)
        {
            case Lifespan.Exclusive:
                if (IsCurrentSessionActive()) return Result.Conflict();
                
                // session can now be started
                
                break;
            case Lifespan.Extended:
                // if extended session no more active, session can now be started
                if (_currentSessionId is null) break;
                
                // end the old session first
                var endSessionService = scope.ServiceProvider.GetRequiredService<IEndSessionService>();
                var ended = await endSessionService.EndSession(_currentSessionId.Value);
                if (!ended.IsSuccess) return Result.Error();
                
                // cleanup after the old session
                _currentSessionId = null;
                _currentConnectionId = null;
                
                // session can now be started
                break;
            default:
                throw new Exception($"Lifespan '{session.Lifespan}' unknown");
        }
        
        // start the session
        var startResult = await startService.StartSession(sessionId);

        if (!startResult.IsSuccess) return Result.Error();
        
        // set the current session and connection
        _currentSessionId = sessionId;
        _currentConnectionId = connectionId;

        return Result.Success();
    }

    /// <summary>
    /// End session operation. Just Ends the active session, releasing it at the end. Connection ownership remains.
    ///
    /// Session's lifespan has no effect here.
    ///
    /// Authorization by both sessionId and connectionId.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="connectionId"></param>
    /// <returns>
    /// - Task Result.Success()
    /// - Task Result.Unauthorized()
    /// - Task Result.Error()
    /// </returns>
    public async Task<Result> EndSessionOfConnection(int sessionId, string connectionId)
    {
        // authorize
        if (!IsCurrentConnection(connectionId).Result.Value || !IsCurrentSession(sessionId).Result.Value)
            return Result.Unauthorized();
        
        // retrieve the ending service
        using var scope = serviceProvider.CreateScope();
        var endSessionService = scope.ServiceProvider.GetRequiredService<IEndSessionService>();

        // end the session
        var ended = await endSessionService.EndSession(sessionId);
        if (!ended.IsSuccess) return Result.Error();

        // release session
        _currentSessionId = null;

        return Result.Success();
    }

    /// <summary>
    /// Leave session operation. Executive function for client disconnections or explicit connection ownership releasing.
    ///
    /// Based on Session's Lifespan, the execution will:
    /// - Exlusive: End the Session first, then release connection and session. 
    /// - Extended: Release connection, the session will remain active.
    /// 
    /// Authorization by connectionId.
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>
    /// - Task Result.Success()
    /// - Task Result.Unauthorized()
    /// - Task Result.Error()
    /// </returns>
    public async Task<Result> LeaveSessionAndConnection(string connectionId)
    {
        // authorize
        if (!IsCurrentConnection(connectionId).Result.Value) return Result.Unauthorized();
        
        // if no session active, just release the connection
        if (_currentSessionId is null)
        {
            _currentConnectionId = null;
            return Result.Success();
        } 
        
        // create scope for a call of this singleton method
        using var scope = serviceProvider.CreateScope();
        
        // retrieve repository
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<SessionAggregate.Session>>();
        
        // retrieve the session entity
        var session = await repository.FirstOrDefaultAsync(new SessionByIdSpec(_currentSessionId.Value));
        if (session is null) return Result.Error();
        
        // lifespan rules
        switch (session.Lifespan)
        {
            case Lifespan.Exclusive:
                var ended = await EndSessionOfConnection(_currentSessionId.Value, connectionId);
                if (!ended.IsSuccess) return Result.Error();
                
                // release connection
                _currentConnectionId = null;
                // release session
                _currentSessionId = null;
                
                break;
            case Lifespan.Extended:
                
                // release connection 
                _currentConnectionId = null;
                
                break;
            default:
                throw new Exception($"Current Lifespan '{session.Lifespan}' unknown");
        }

        return Result.Success();
    }

    /// <summary>
    /// Comparator method, if current (active or null) connection equals the one in param.
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>
    /// - Task Result.Success() [bool] 
    /// </returns>
    public Task<Result<bool>> IsCurrentConnection(string connectionId)
    {
        var result = Result.Success(_currentConnectionId == connectionId);
        return Task.FromResult(result);
    }

    /// <summary>
    /// Comparator method, if current (active or null) session equals the one in param.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>
    /// - Task Result.Success() [bool] 
    /// </returns>
    public Task<Result<bool>> IsCurrentSession(int sessionId)
    {
        var result = Result.Success(_currentSessionId == sessionId);
        return Task.FromResult(result);
    }
    
    private bool IsCurrentConnectionActive()
    {
        return _currentConnectionId is not null;
    }
    
    private bool IsCurrentSessionActive()
    {
        return _currentSessionId is not null;
    }
}