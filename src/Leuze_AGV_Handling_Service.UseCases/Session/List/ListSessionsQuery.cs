using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.List;

public record ListSessionsQuery() : IQuery<Result<IEnumerable<SessionDTO>>>;
