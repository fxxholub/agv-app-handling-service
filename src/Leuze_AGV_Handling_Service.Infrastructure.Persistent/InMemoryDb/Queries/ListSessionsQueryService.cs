using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.List;
using Microsoft.EntityFrameworkCore;

namespace Leuze_AGV_Handling_Service.Infrastructure.Persistent.InMemoryDb.Queries;

public class ListSessionsQueryService(AppDbContext db) : IListSessionsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries -
  // this is just an example

  public async Task<IEnumerable<Session>> ListAsync()
  {
    // NOTE: This will fail if testing with EF InMemory provider!
    // var result = await _db.Database.SqlQuery<ContributorDTO>(
    //   $"SELECT Id, Name, PhoneNumber_Number AS PhoneNumber FROM Contributors") // don't fetch other big columns
    //   .ToListAsync();
    var sessions = await db.Sessions
      .Include(sesh => sesh.Actions)
      .Include(sesh => sesh.Processes)
      .ToListAsync();

    return sessions;
  }
}
