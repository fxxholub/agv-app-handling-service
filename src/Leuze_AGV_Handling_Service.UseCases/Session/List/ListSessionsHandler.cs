using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Contributors.List;

namespace Leuze_AGV_Handling_Service.UseCases.Session.List;

public class ListSessionsHandler(IListSessionsQueryService query)
  : IQueryHandler<ListSessionsQuery, Result<IEnumerable<SessionDTO>>>
{
  public async Task<Result<IEnumerable<SessionDTO>>> Handle(ListSessionsQuery request, CancellationToken cancellationToken)
  {
    var result = await query.ListAsync();

    return Result.Success();
  }
}
