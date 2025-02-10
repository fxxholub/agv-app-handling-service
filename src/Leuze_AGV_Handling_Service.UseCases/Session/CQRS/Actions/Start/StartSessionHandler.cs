using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Start;

/// <summary>
/// Starts Session. Effectivaly starts its processes, checks if they started successfully and marks session as Started.
/// </summary>
/// <param name="sessionExecutor"></param>
public class StartSessionHandler(ISessionExecutorService sessionExecutor, ILogger<StartSessionHandler> logger, IMediator mediator)
  : ICommandHandler<StartSessionCommand, Result>
{
  public async Task<Result> Handle(StartSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var getResult = await mediator.Send(new GetSessionQuery(request.SessionId), cancellationToken);

      if (getResult.Value.HandlingMode != request.HandlingMode)
      {
        return Result.Invalid(new ValidationError($"Session with id {request.SessionId} does not match the requested handling mode, {request.HandlingMode}."));
      }
      
      var startResult =  await sessionExecutor.StartSessionAndReserveConnection(request.SessionId, request.ConnectionId);
      
      if (startResult.IsSuccess)
      {
        var agvMode = request.HandlingMode.ToString().ToLower();
        await mediator.Publish(new AgvMode(agvMode), cancellationToken);
      }

      return startResult;
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} Session {request.SessionId} start error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
