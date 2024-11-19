using Ardalis.Specification;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Specifications;

/// <summary>
/// Specification fetching Session by its ID with its cascade Actions and Processes.
/// </summary>
public sealed class SessionByIdSpec : Specification<Session>
{
  public SessionByIdSpec(int sessionId)
  {
      Query
          .Where(sesh => sesh.Id == sessionId);
  }
}
