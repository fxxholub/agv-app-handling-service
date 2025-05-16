using Ardalis.Result;

namespace Handling_Service.Core.Session.Interfaces;

public interface IStartSessionService
{
  public Task<Result> StartSession(int sessionId);
}
