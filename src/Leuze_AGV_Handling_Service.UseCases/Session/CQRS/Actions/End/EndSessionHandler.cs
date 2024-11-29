using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.End;

/// <summary>
/// Ends Session. Effectively kills its processes and marks it as Ended.
/// </summary>
/// <param name="sessionExecutor"></param>
public class EndSessionHandler(ISessionExecutorService sessionExecutor, ILogger<EndSessionHandler> logger)
  : ICommandHandler<EndSessionCommand, Result>
{
  public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await sessionExecutor.EndSessionOfConnection(request.SessionId, request.ConnectionId);
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} Session {request.SessionId} end error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
