using Ardalis.Result;

namespace Handling_Service.Core.Session.Interfaces;

public interface ICheckSessionService
{
  public Task<Result<bool>> CheckSession(int sessionId);
}
