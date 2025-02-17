using Ardalis.Specification;

namespace Handling_Service.Core.Session.SessionAggregate.Specifications;

/// <summary>
/// Specification fetching Session by its ID with its cascade Actions and Processes, which contains its cascade Commands.
/// </summary>
public sealed class SessionByIdWithActionsAndProcessesWithCommandsSpec : Specification<Session>
{
  public SessionByIdWithActionsAndProcessesWithCommandsSpec(int sessionId)
  {
    Query
        .Where(sesh => sesh.Id == sessionId)
        .Include(session => session.Actions)
        .Include(session => session.Processes)
        .ThenInclude(p => p.Commands);
  }
}
