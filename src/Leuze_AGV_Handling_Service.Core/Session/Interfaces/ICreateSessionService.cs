using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface ICreateSessionService
{
  public Task<Result<SessionAggregate.Session>> CreateSession(
    HandlingMode handlingMode,
    bool mappingEnabled,
    string? inputMapRef,
    string? outputMapRef,
    string? outputMapName
  );
}
