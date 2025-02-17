using Ardalis.Result;

namespace Handling_Service.Core.Session.Interfaces;

public interface IDeleteSessionService
{
  public Task<Result> DeleteSession(int sessionId);
}
