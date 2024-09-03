using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface ICheckSessionService
{
  public Task<Result> CheckSession(int sessionId);
}
