using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.GetCurrent;

public record GetCurrentSessionQuery() : IQuery<Result<SessionDto>>;
