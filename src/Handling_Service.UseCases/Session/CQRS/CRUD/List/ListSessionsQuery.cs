using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.UseCases.Session.DTOs;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.List;

/// <summary>
/// Fetches multiple Sessions. Safe repository operation. Does not change anything in the repository nor does it trigger any events.
/// </summary>
public record ListSessionsQuery() : IQuery<Result<IEnumerable<SessionDto>>>;
