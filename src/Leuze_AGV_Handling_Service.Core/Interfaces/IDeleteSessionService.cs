using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface IDeleteSessionService
{
  public Task<Result> DeleteSession(int sessionId);
}
