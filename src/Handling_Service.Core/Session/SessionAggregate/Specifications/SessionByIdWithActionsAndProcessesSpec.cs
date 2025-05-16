using Ardalis.Specification;

namespace Handling_Service.Core.Session.SessionAggregate.Specifications;

/// <summary>
/// Specification fetching Session by its ID with its cascade Actions and Processes.
/// </summary>
public sealed class SessionByIdWithActionsAndProcessesSpec : Specification<Session>
{
  public SessionByIdWithActionsAndProcessesSpec(int sessionId)
  {
    Query
        .Where(sesh => sesh.Id == sessionId)
        .Include(session => session.Actions)
        .Include(session => session.Processes);
  }
}
