using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.List;

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
        entity.State,
        entity.Processes.Select(process => new ProcessDTO(
          process.Name,
          process.HostName,
          process.HostAddr,
          process.UserName,
          process.SessionId,
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
