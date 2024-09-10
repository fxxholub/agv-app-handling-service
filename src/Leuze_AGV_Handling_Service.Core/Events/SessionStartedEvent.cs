using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Events;

/// <summary>
/// Event notifying system about started session.
/// </summary>
/// <param name="sessionId"></param>
internal sealed class SessionStartedEvent(int sessionId, HandlingMode sessionMode) : DomainEventBase
{
  public int SessionId { get; init; } = sessionId;
  public HandlingMode SessionMode { get; init; } = sessionMode;
}
