using Ardalis.Result;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.Core.Session.Interfaces;

public interface ICreateSessionService
{
  public Task<Result<int>> CreateSession(HandlingMode handlingMode, Lifespan lifespan);
}
