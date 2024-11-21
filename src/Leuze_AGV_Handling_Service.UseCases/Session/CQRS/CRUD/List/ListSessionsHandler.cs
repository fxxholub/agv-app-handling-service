using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.List;

/// <summary>
/// Fetches multiple Sessions. Safe repository operation. Does not change anything in the repository nor does it trigger any events.
/// </summary>
/// <param name="query"></param>
public class ListSessionsHandler(IListSessionsQueryService query)
  : IQueryHandler<ListSessionsQuery, Result<IEnumerable<SessionDto>>>
{
  public async Task<Result<IEnumerable<SessionDto>>> Handle(ListSessionsQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var sessions = await query.ListAsync();
      
      var result = sessions.Select(entity => 
        new SessionDto(
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
        )
      );

      return Result.Success(result);
    }
    catch
    {
      return Result.Error(new ErrorList(["Unknown error requesting list all sessions."]));
    }
  }
}
