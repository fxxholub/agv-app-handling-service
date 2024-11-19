using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;

/// <summary>
/// Fetches Session. Safe operation.
/// </summary>
/// <param name="repository"></param>
public class IsCurrentConnectionHandler(ISessionExecutorService sessionExecutor)
  : IQueryHandler<IsCurrentConnectionQuery, Result<bool>>
{
  public async Task<Result<bool>> Handle(IsCurrentConnectionQuery request, CancellationToken cancellationToken)
  {
    return await sessionExecutor.IsCurrentConnection(request.ConnectionId);
  }
}
