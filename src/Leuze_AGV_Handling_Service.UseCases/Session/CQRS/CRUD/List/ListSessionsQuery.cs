using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.List;

/// <summary>
/// Fetches multiple Sessions. Safe repository operation. Does not change anything in the repository nor does it trigger any events.
/// </summary>
public record ListSessionsQuery() : IQuery<Result<IEnumerable<SessionDto>>>;
