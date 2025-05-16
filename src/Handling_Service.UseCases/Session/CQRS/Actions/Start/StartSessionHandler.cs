using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Messaging.Topics;
using Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Handling_Service.UseCases.Session.CQRS.Actions.Start;

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
      var startResult =  await sessionExecutor.StartSessionAndReserveConnection(
        request.ConnectionId,
        request.HandlingMode,
        request.Mapping
        );

      return startResult;
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} Session start error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
