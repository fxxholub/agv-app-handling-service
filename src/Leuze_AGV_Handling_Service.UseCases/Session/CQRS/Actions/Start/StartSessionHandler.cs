using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Start;

/// <summary>
/// Starts Session. Effectivaly starts its processes, checks if they started successfully and marks session as Started.
/// </summary>
/// <param name="sessionExecutor"></param>
public class StartSessionHandler(ISessionExecutorService sessionExecutor, ILogger<StartSessionHandler> logger)
  : ICommandHandler<StartSessionCommand, Result>
{
  public async Task<Result> Handle(StartSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await sessionExecutor.StartSessionAndReserveConnection(request.SessionId, request.ConnectionId);
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} Session {request.SessionId} start error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
