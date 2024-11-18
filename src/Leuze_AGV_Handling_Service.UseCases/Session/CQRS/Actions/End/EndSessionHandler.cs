using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.End;

/// <summary>
/// Ends Session.
/// </summary>
/// <param name="sessionExecutor"></param>
public class EndSessionHandler(ISessionExecutorService sessionExecutor)
  : ICommandHandler<EndSessionCommand, Result>
{
  public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
  {
    return await sessionExecutor.EndSessionOfConnection(request.SessionId, request.ConnectionId);
  }
}
