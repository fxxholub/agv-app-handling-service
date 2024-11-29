using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Leave;

/// <summary>
/// Leaves Session. Releases connection ownership. To be used in OnDisconnected methods or for manual releasing.
/// </summary>
/// <param name="sessionExecutor"></param>
public class LeaveSessionHandler(ISessionExecutorService sessionExecutor, ILogger<LeaveSessionHandler> logger)
  : ICommandHandler<LeaveSessionCommand, Result>
{
  public async Task<Result> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await sessionExecutor.LeaveSessionAndConnection(request.ConnectionId);
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} session leave error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
