using Ardalis.Result;

namespace Handling_Service.Core.Session.Interfaces;

public interface IEndSessionService
{
  public Task<Result> EndSession(int sessionId);
}
