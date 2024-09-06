using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Get;

public record GetSessionQuery(int SessionId) : IQuery<Result<SessionDTO>>;
