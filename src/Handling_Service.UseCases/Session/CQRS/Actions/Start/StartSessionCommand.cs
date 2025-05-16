using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.UseCases.Session.CQRS.Actions.Start;

/// <summary>
/// Starts Session. Effectivaly starts its processes, checks if they started successfully and marks session as Started.
/// </summary>
/// <param name="ConnectionId"></param>
/// <param name="HandlingMode"></param>
/// <param name="Mapping"></param>
public record StartSessionCommand(string ConnectionId, HandlingMode HandlingMode, bool Mapping) : ICommand<Result>;
