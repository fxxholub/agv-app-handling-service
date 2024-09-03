using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface IStartSessionService
{
  public Task<Result> StartSession(int sessionId);
}
