using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Core.Session.Services;

/// <summary>
/// Common Manager for other Session services - keeps just one (current) Session alive.
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
        var startSessionService = scope.ServiceProvider.GetRequiredService<IStartSessionService>();
        var endSessionService = scope.ServiceProvider.GetRequiredService<IEndSessionService>();
            
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

        // start the session along with its processes
        var started = await startSessionService.StartSession(created.Value);

        // if start didn go well, rollback the start transaction by ending it
        if (!started.IsSuccess)
        {
            await endSessionService.EndSession(created.Value);
            return Result.Error();
        } 

        // set the session as current
        _currentSessionId = created.Value;
        _currentHandlingMode = handlingMode;
        
        // enable message channel
        _enableMessageChannel();

        return Result.Success(created.Value);
    }

    public async Task<Result> EndAndDeleteSession(int sessionId)
    {
        // retrieve the services
        using var scope = serviceProvider.CreateScope();
        var endSessionService = scope.ServiceProvider.GetRequiredService<IEndSessionService>();
        var deleteSessionService = scope.ServiceProvider.GetRequiredService<IDeleteSessionService>();
        
        if (_isCurrentSession(sessionId))
        {
            // if session is current session, first end it
            var ended = await endSessionService.EndSession(sessionId);
            if (!ended.IsSuccess) return Result.Error();
            
            // disable message channel
            _disableMessageChannel();
            
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

    public async Task<Result<bool>> CheckAndEndBadSession(int sessionId)
    {
        // retrieve the services
        using var scope = serviceProvider.CreateScope();
        var checkSessionService = scope.ServiceProvider.GetRequiredService<ICheckSessionService>();
        
        if (_isCurrentSession(sessionId))
        {
            var checkedResult = await checkSessionService.CheckSession(sessionId);
            if (!checkedResult.IsSuccess) return Result.Error();
            
            //TODO the end part
            
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