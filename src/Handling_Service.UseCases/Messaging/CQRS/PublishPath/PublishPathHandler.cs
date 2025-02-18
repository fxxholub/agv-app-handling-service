using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;
using Path = Handling_Service.UseCases.Messaging.Topics.Path;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishPath;

/// <summary>
/// Creates Session. Entity repository creation operation.
/// </summary>
/// <param name="mediator"></param>
public class PublishPathHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<PublishPathCommand, Result>
{
  public async Task<Result> Handle(PublishPathCommand command, CancellationToken cancellationToken)
  {
    var currentHandlingMode = sessionExecutor.CurrentHandlingMode();
    var isCurrentConnection = sessionExecutor.IsCurrentConnection(command.connectionId).Result.Value;
    
    if (
      isCurrentConnection &&
      currentHandlingMode == HandlingMode.Automatic
      )
    {
      await mediator.Publish(new Path(command.path), cancellationToken);
    }
    return new Result();
  }
}
