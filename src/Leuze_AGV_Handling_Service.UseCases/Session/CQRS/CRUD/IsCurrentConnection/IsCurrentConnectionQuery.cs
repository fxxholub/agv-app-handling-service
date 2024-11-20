using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.IsCurrentConnection;

/// <summary>
/// Boolean test if connection is current active connection (if any, else returns false).
/// </summary>
/// <param name="ConnectionId"></param>
public record IsCurrentConnectionQuery(string ConnectionId) : IQuery<Result<bool>>;
