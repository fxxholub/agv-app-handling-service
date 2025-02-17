using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.UseCases.Session.CQRS.Actions.Start;

/// <summary>
/// Starts Session. Effectivaly starts its processes, checks if they started successfully and marks session as Started.
/// </summary>
/// <param name="SessionId"></param>
/// <param name="ConnectionId"></param>
public record StartSessionCommand(int SessionId, string ConnectionId, HandlingMode HandlingMode) : ICommand<Result>;
