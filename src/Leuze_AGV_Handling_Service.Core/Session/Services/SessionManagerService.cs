using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Common Manager Service for other Session services.
/// Guarantees that only one actively running session exists. 
/// Handles operations chaining
/// (for example Session Create and attempt to Start; Session Delete with session Ending).
/// </summary>
/// <param name="serviceProvider"></param>
public class SessionManagerService(
    IServiceProvider serviceProvider,
    IAutonomousMessageChannel autonomousChannel,
    IManualMessageChannel manualChannel
    ) : ISessionManagerService
{
    private int? _currentSessionId = null;
    private HandlingMode? _currentHandlingMode = null;

    /// <summary>
    /// Creates Session, attempts to start it. Will proceed only if no current session is active.
    /// </summary>
    /// <param name="handlingMode"></param>
    /// <param name="mappingEnabled"></param>
    /// <param name="inputMapRef"></param>
    /// <param name="outputMapRef"></param>
    /// <param name="outputMapName"></param>
    /// <returns></returns>
    public async Task<Result<int>> CreateAndStartSession(
        HandlingMode handlingMode,
        bool mappingEnabled,
        string? inputMapRef,
        string? outputMapRef,
        string? outputMapName
        )
    {
        // retrieve the services
        using var scope = serviceProvider.CreateScope();
        var createSessionService = scope.ServiceProvider.GetRequiredService<ICreateSessionService>();
            
        if (_currentSessionExists()) return Result.Conflict();

        // create the session
        var created = await createSessionService.CreateSession(
            handlingMode,
            mappingEnabled,
            inputMapRef,
            outputMapRef,
            outputMapName
        );

        if (!created.IsSuccess) return Result.Error();
        
        // set the session as current
        _currentSessionId = created.Value;
        _currentHandlingMode = handlingMode;
        
        // additionally attempt to start the session
        await StartSession(created.Value);

        return Result.Success(created.Value);
    }

    /// <summary>
    /// Deletes the session, But if the Session is current active session, Ends it first.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public async Task<Result> EndAndDeleteSession(int sessionId)
    {
        // retrieve the services
        using var scope = serviceProvider.CreateScope();
        var deleteSessionService = scope.ServiceProvider.GetRequiredService<IDeleteSessionService>();
        
        // end the session first
        var ended = await EndSession(sessionId);
        
        if (!ended.IsSuccess) return Result.Error();
        
        // finally, delete the session
        var deleted = await deleteSessionService.DeleteSession(sessionId);
        
        if (!deleted.IsSuccess) return Result.Error();

        return Result.Success();
    }

    /// <summary>
    /// Starts Session if it is the current active Session.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public async Task<Result> StartSession(int sessionId)
    {
        // retrieve the services
        using var scope = serviceProvider.CreateScope();
        var startSessionService = scope.ServiceProvider.GetRequiredService<IStartSessionService>();
        
        if (_isCurrentSession(sessionId))
        {
            // start the session along with its processes
            var started = await startSessionService.StartSession(sessionId);

            if (!started.IsSuccess)
            {
                return Result.Error();
            }
        
            // enable message channel
            _enableMessageChannel();

            return Result.Success();
        }

        return Result.Conflict();
    }

    /// <summary>
    /// Ends session if it is the current active Session.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public async Task<Result> EndSession(int sessionId)
    {
        // retrieve the services
        using var scope = serviceProvider.CreateScope();
        var endSessionService = scope.ServiceProvider.GetRequiredService<IEndSessionService>();
        
        if (_isCurrentSession(sessionId))
        {
            var ended = await endSessionService.EndSession(sessionId);
            if (!ended.IsSuccess) return Result.Error();
            
            // disable message channel
            _disableMessageChannel();
            
            // right after session ends, new session could be made
            _currentSessionId = null;

            return Result.Success();
        }

        return Result.Conflict();
    }

    /// <summary>
    /// Checks the Session and Ends it if check is bad.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public Task<Result<bool>> CheckAndEndBadSession(int sessionId)
    {
        throw new NotImplementedException();
        // // retrieve the services
        // using var scope = serviceProvider.CreateScope();
        // var checkSessionService = scope.ServiceProvider.GetRequiredService<ICheckSessionService>();
        //
        // if (_isCurrentSession(sessionId))
        // {
        //     var checkedResult = await checkSessionService.CheckSession(sessionId);
        //     if (!checkedResult.IsSuccess) return Result.Error();
        //     
        //     //TODO the end part
        //     
        //     return Result.Success(checkedResult.Value);
        // }
        //
        // return Result.Conflict();
    }
    
    private bool _currentSessionExists()
    {
        return _currentSessionId is not null;
    }
    
    private bool _isCurrentSession(int sessionId)
    {
        return _currentSessionId is not null;
    }

    private async void _enableMessageChannel()
    {
        if (_currentHandlingMode is HandlingMode.Autonomous)
        {
            await autonomousChannel.Enable();
        } else if (_currentHandlingMode is HandlingMode.Manual)
        {
            await manualChannel.Enable();
        }
    }

    private async void _disableMessageChannel()
    {
        if (_currentHandlingMode is HandlingMode.Autonomous)
        {
            await autonomousChannel.Disable();
        } else if (_currentHandlingMode is HandlingMode.Manual)
        {
            await manualChannel.Disable();
        }
    }
}