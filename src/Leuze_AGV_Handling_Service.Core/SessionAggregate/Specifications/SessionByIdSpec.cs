using Ardalis.Specification;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Specifications;

public class SessionByIdSpec : Specification<Session>
{
  public SessionByIdSpec(int sessionId)
  {
    Query
        .Where(sesh => sesh.Id == sessionId);
  }
}
