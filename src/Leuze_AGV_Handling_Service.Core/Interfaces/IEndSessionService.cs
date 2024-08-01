using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface IEndSessionService
{
  public Task<Result> EndSession(int sessionId);
}
