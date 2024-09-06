using Ardalis.Specification;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Specifications;

public sealed class SessionByIdWithProcessesSpec : Specification<Session>
{
  public SessionByIdWithProcessesSpec(int sessionId)
  {
    Query
        .Where(sesh => sesh.Id == sessionId)
        .Include(session => session.Processes);
  }
}
