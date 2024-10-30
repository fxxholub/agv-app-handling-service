using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface IEndSessionService
{
  public Task<Result> EndSession(int sessionId);
}
