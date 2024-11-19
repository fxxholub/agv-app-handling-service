using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Get;

public record GetSessionQuery(int SessionId) : IQuery<Result<SessionDto>>;
