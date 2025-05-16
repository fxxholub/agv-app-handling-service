using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Handling_Service.UseCases.Session.CQRS.Actions.Leave;

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
      var leaveResult = await sessionExecutor.LeaveSessionAndConnection(request.ConnectionId);

      return leaveResult;
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} session leave error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
