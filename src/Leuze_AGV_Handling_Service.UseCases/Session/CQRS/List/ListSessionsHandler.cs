using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.List;

/// <summary>
/// Lists Sessions. Safe operation.
/// </summary>
/// <param name="query"></param>
public class ListSessionsHandler(IListSessionsQueryService query)
  : IQueryHandler<ListSessionsQuery, Result<IEnumerable<SessionDTO>>>
{
  public async Task<Result<IEnumerable<SessionDTO>>> Handle(ListSessionsQuery request, CancellationToken cancellationToken)
  {
    var sessions = await query.ListAsync();
    
    var result = sessions.Select(entity => 
      new SessionDTO(
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
      )
    );

    return Result.Success(result);
  }
}
