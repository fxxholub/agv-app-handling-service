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
public class EndSessionHandler(ISessionExecutorService sessionExecutor, ILogger<EndSessionHandler> logger, IMediator mediator)
  : ICommandHandler<EndSessionCommand, Result>
{
  public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var endResult = await sessionExecutor.EndSessionOfConnection(request.SessionId, request.ConnectionId);
      
      if (endResult.IsSuccess)
      {
        await mediator.Publish(new AgvMode(""), cancellationToken);
      }

      return endResult;
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} Session {request.SessionId} end error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
