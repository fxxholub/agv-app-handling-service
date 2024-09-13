using Ardalis.Specification;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;

/// <summary>
/// Specification fetching Session by its ID with its cascade Processes.
/// </summary>
public sealed class SessionByIdWithProcessesSpec : Specification<Session>
{
  public SessionByIdWithProcessesSpec(int sessionId)
  {
    Query
        .Where(sesh => sesh.Id == sessionId)
        .Include(session => session.Processes);
  }
}
