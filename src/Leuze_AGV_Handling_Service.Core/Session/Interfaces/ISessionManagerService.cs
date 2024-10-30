using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface ISessionManagerService
{
    public Task<Result<int>> CreateSession(
        HandlingMode handlingMode,
        bool mappingEnabled,
        string? inputMapRef,
        string? outputMapRef,
        string? outputMapName
    );

    public Task<Result> StartSession(int sessionId);

    public Task<Result> EndSession(int sessionId);
    
    public Task<Result<bool>> CheckSession(int sessionId);

    public Task<Result> DeleteSession(int sessionId);

    public Result<int> GetCurrentSessionId();

    public bool CurrentSessionExists();
}