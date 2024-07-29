using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Specifications;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Get;

public class GetSessionHandler(IReadRepository<Core.SessionAggregate.Session> repository)
  : IQueryHandler<GetSessionQuery, Result<SessionDTO>>
{
  public async Task<Result<SessionDTO>> Handle(GetSessionQuery request, CancellationToken cancellationToken)
  {
    var spec = new SessionByIdSpec(request.SessionId);
    var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new SessionDTO(
      entity.Id,
      entity.HandlingMode,
      entity.MappingEnabled,
      entity.InputMapRef ?? "",
      entity.OutputMapRef ?? "",
      entity.OutputMapName ?? "",
      entity.State,
      entity.Processes.Select(process => new ProcessDTO(
        process.Name,
        process.SessionId,
        process.Pid,
        process.State,
        process.CreatedDate
        )).ToList(),
      entity.CreatedDate
      );
  }
}
