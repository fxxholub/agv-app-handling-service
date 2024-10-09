using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.GetCurrent;

/// <summary>
/// Fetches Current Session. Safe operation.
/// </summary>
/// <param name="repository"></param>
public class GetCurrentSessionHandler(
  ISessionManagerService sessionManager,
  IRepository<Core.Session.SessionAggregate.Session> repository)
  : IQueryHandler<GetCurrentSessionQuery, Result<SessionDto>>
{
  public async Task<Result<SessionDto>> Handle(GetCurrentSessionQuery request, CancellationToken cancellationToken)
  {
    var result = sessionManager.GetCurrentSessionId();

    if (!result.IsSuccess)
    {
      return Result.NotFound();
    }
    
    var spec = new SessionByIdWithActionsAndProcessesSpec(result.Value);
    var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new SessionDto(
      entity.Id,
      entity.HandlingMode,
      entity.MappingEnabled,
      entity.InputMapRef ?? "",
      entity.OutputMapRef ?? "",
      entity.OutputMapName ?? "",
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
}
