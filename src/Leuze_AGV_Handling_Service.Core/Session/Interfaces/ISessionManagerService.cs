using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface ISessionManagerService
{
    public Task<Result<SessionAggregate.Session>> CreateAndStartSession(
        HandlingMode handlingMode,
        bool mappingEnabled,
        string? inputMapRef,
        string? outputMapRef,
        string? outputMapName
        );
    
    public Task<Result> EndAndDeleteSession(int sessionId);
    
    public Task<Result> EndSession(int sessionId);

    public Task<Result<bool>> CheckAndEndBadSession(int sessionId);
}