using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Get;

/// <summary>
/// Fetches Session. Safe repository operation. Does not change anything in the repository nor does it trigger any events.
/// </summary>
/// <param name="repository"></param>
public class GetSessionHandler(IRepository<Core.Session.SessionAggregate.Session> repository, ILogger<GetSessionHandler> logger)
  : IQueryHandler<GetSessionQuery, Result<SessionDto>>
{
  public async Task<Result<SessionDto>> Handle(GetSessionQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var spec = new SessionByIdWithActionsAndProcessesSpec(request.SessionId);
      var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);
      if (entity is null) return Result.NotFound();

      return new SessionDto(
        entity.Id,
        entity.HandlingMode,
        entity.ErrorReason,
        entity.State,
        entity.Actions.Select(action => new ActionDto(
          action.Command,
          action.SessionId,
          action.CreatedDate
          )).ToList(),
        entity.Processes.Select(process => new ProcessDto(
          process.Name,
          process.HostName,
          process.HostAddr,
          process.UserName,
          process.SessionId,
          process.ErrorReason,
          process.Pid,
          process.State,
          process.CreatedDate
        )).ToList(),
        entity.CreatedDate
      );
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Session {request.SessionId} get error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
