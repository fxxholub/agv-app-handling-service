using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.List;

public class ListSessionsHandler(IListSessionsQueryService query, ILogger<ListSessionsHandler> logger)
  : IQueryHandler<ListSessionsQuery, Result<IEnumerable<SessionDTO>>>
{
  public async Task<Result<IEnumerable<SessionDTO>>> Handle(ListSessionsQuery request, CancellationToken cancellationToken)
  {
    var result = await query.ListAsync();
    logger.LogInformation("list happened");

    return Result<IEnumerable<SessionDTO>>.Success(result);
  }
}
