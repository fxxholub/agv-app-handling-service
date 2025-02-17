using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Handling_Service.UseCases.Session.CQRS.Actions.End;

/// <summary>
/// Ends Session. Effectively kills its processes and marks it as Ended.
/// </summary>
/// <param name="SessionId"></param>
/// <param name="ConnectionId"></param>
public record EndSessionCommand(int SessionId, string ConnectionId) : ICommand<Result>;
