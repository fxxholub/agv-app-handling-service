using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Leave;

/// <summary>
/// Leaves Session. Releases connection ownership. To be used in OnDisconnected methods or for manual releasing.
/// </summary>
/// <param name="sessionExecutor"></param>
public class LeaveSessionHandler(ISessionExecutorService sessionExecutor)
  : ICommandHandler<LeaveSessionCommand, Result>
{
  public async Task<Result> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await sessionExecutor.LeaveSessionAndConnection(request.ConnectionId);
    }
    catch
    {
      return Result.Error(new ErrorList(["Unknown error executing leave session."]));
    }
  }
}
