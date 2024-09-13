using Ardalis.Specification;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;

/// <summary>
/// Specification fetching Session by its ID with its cascade Processes, which contains its cascade Commands.
/// </summary>
public sealed class SessionByIdWithProcessesWithCommandsSpec : Specification<Session>
{
  public SessionByIdWithProcessesWithCommandsSpec(int sessionId)
  {
    Query
        .Where(sesh => sesh.Id == sessionId)
        .Include(session => session.Processes)
        .ThenInclude(p => p.Commands);
  }
}
