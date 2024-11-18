using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface ICreateSessionService
{
  public Task<Result<int>> CreateSession(HandlingMode handlingMode, Lifespan lifespan);
}
