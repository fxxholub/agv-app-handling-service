using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;

public record IsCurrentConnectionQuery(string ConnectionId) : IQuery<Result<bool>>;
