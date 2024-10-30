using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface ICheckSessionService
{
  public Task<Result<bool>> CheckSession(int sessionId);
}
