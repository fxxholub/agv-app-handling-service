using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;

/// <summary>
/// Boolean test if connection is current active connection (if any, else returns false).
/// </summary>
/// <param name="sessionExecutor"></param>
public class IsCurrentConnectionHandler(ISessionExecutorService sessionExecutor)
  : IQueryHandler<IsCurrentConnectionQuery, Result<bool>>
{
  public async Task<Result<bool>> Handle(IsCurrentConnectionQuery request, CancellationToken cancellationToken)
  {
    try
    {
      return await sessionExecutor.IsCurrentConnection(request.ConnectionId);
    }
    catch
    {
      return Result.Error(new ErrorList(["Unknown error requesting IsCurrentConnection."]));
    }
  }
}
