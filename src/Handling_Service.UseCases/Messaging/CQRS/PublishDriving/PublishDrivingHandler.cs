using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.UseCases.Messaging.CQRS.PublishDriving;

/// <summary>
/// Publishes driving flag
/// </summary>
/// <param name="mediator"></param>
public class PublishDrivingHandler(ISessionExecutorService sessionExecutor, IMediator mediator) : ICommandHandler<PublishDrivingCommand, Result>
{
  public async Task<Result> Handle(PublishDrivingCommand command, CancellationToken cancellationToken)
  {
    var currentHandlingMode = sessionExecutor.CurrentHandlingMode();
    var isCurrentConnection = sessionExecutor.IsCurrentConnection(command.connectionId).Result.Value;
    
    if (
      isCurrentConnection &&
      currentHandlingMode == HandlingMode.Automatic
      )
    {
      await mediator.Publish(new DrivingTopic(command.driving), cancellationToken);
    }
    return new Result();
  }
}
