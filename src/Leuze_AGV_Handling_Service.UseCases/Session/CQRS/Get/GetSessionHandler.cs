using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Get;

/// <summary>
/// Fetches Session. Safe operation.
/// </summary>
/// <param name="repository"></param>
public class GetSessionHandler(IRepository<Core.Session.SessionAggregate.Session> repository)
  : IQueryHandler<GetSessionQuery, Result<SessionDTO>>
{
  public async Task<Result<SessionDTO>> Handle(GetSessionQuery request, CancellationToken cancellationToken)
  {
    var spec = new SessionByIdWithProcessesSpec(request.SessionId);
    var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new SessionDTO(
      entity.Id,
      entity.HandlingMode,
      entity.MappingEnabled,
      entity.InputMapRef ?? "",
      entity.OutputMapRef ?? "",
      entity.OutputMapName ?? "",
      entity.ErrorReason,
      entity.State,
      entity.Processes.Select(process => new ProcessDTO(
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
