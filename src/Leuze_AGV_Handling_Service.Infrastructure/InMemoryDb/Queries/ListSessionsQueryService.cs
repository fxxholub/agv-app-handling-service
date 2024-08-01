using Ardalis.Specification.EntityFrameworkCore;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Contributors;
using Leuze_AGV_Handling_Service.UseCases.Contributors.List;
using Leuze_AGV_Handling_Service.UseCases.Session;
using Leuze_AGV_Handling_Service.UseCases.Session.List;
using Microsoft.EntityFrameworkCore;

namespace Leuze_AGV_Handling_Service.Infrastructure.InMemoryDb.Queries;

public class ListSessionsQueryService(AppDbContext _db) : IListSessionsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries -
  // this is just an example

  public async Task<IEnumerable<SessionDTO>> ListAsync()
  {
    // NOTE: This will fail if testing with EF InMemory provider!
    // var result = await _db.Database.SqlQuery<ContributorDTO>(
    //   $"SELECT Id, Name, PhoneNumber_Number AS PhoneNumber FROM Contributors") // don't fetch other big columns
    //   .ToListAsync();
    var sessions = await _db.Sessions.ToListAsync();

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
            process.SessionId,
            process.Pid,
            process.State,
            process.CreatedDate
          )).ToList(),
          entity.CreatedDate
        )
      );

    return result;
  }
}
