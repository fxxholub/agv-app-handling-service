using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishCmdVel;

/// <summary>
/// Creates Session. Entity repository creation operation.
/// </summary>
/// <param name="mediator"></param>
public class PublishCmdVelHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<PublishCmdVelCommand, Result>
{
  public async Task<Result> Handle(PublishCmdVelCommand command, CancellationToken cancellationToken)
  {
    var currentHandlingMode = sessionExecutor.CurrentHandlingMode();
    var isCurrentConnection = sessionExecutor.IsCurrentConnection(command.connectionId).Result.Value;
    
    if (
      isCurrentConnection &&
      currentHandlingMode == HandlingMode.Manual
      )
    {
      await mediator.Publish(new CmdVel(command.data), cancellationToken);
    }
    return new Result();
  }
}
