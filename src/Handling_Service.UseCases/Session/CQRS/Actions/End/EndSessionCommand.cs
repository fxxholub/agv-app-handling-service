using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Handling_Service.UseCases.Session.CQRS.Actions.End;

/// <summary>
/// Ends Session. Effectively kills its processes and marks it as Ended.
/// </summary>
/// <param name="ConnectionId"></param>
public record EndSessionCommand(string ConnectionId) : ICommand<Result>;
