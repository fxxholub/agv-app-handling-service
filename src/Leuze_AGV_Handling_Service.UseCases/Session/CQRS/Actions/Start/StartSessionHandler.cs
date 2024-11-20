using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Start;

/// <summary>
/// Starts Session. Effectivaly starts its processes, checks if they started successfully and marks session as Started.
/// </summary>
/// <param name="sessionExecutor"></param>
public class StartSessionHandler(ISessionExecutorService sessionExecutor)
  : ICommandHandler<StartSessionCommand, Result>
{
  public async Task<Result> Handle(StartSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await sessionExecutor.StartSessionAndReserveConnection(request.SessionId, request.ConnectionId);
    }
    catch
    {
      return Result.Error(new ErrorList(["Unknown error executing start session."]));
    }
  }
}
