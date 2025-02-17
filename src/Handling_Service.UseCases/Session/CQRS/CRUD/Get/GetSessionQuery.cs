using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.UseCases.Session.DTOs;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.Get;

/// <summary>
/// Fetches Session. Safe repository operation. Does not change anything in the repository nor does it trigger any events.
/// </summary>
/// <param name="SessionId"></param>
public record GetSessionQuery(int SessionId) : IQuery<Result<SessionDto>>;
