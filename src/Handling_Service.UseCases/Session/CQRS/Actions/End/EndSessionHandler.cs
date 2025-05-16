using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Handling_Service.UseCases.Session.CQRS.Actions.End;

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
      var endResult = await sessionExecutor.EndSessionOfConnection(request.ConnectionId);

      return endResult;
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} Session end error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
