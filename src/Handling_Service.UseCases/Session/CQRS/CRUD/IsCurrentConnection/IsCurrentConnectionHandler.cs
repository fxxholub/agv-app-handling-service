using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;

/// <summary>
/// Boolean test if connection is current active connection (if any, else returns false).
/// </summary>
/// <param name="sessionExecutor"></param>
public class IsCurrentConnectionHandler(ISessionExecutorService sessionExecutor, ILogger<IsCurrentConnectionHandler> logger)
  : IQueryHandler<IsCurrentConnectionQuery, Result<bool>>
{
  public async Task<Result<bool>> Handle(IsCurrentConnectionQuery request, CancellationToken cancellationToken)
  {
    try
    {
      return await sessionExecutor.IsCurrentConnection(request.ConnectionId);
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Connection {request.ConnectionId} is current connection error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
