using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;

/// <summary>
/// Event notifying system about started session.
/// </summary>
/// <param name="sessionId"></param>
internal sealed class SessionStartedEvent(int sessionId, HandlingMode sessionMode) : DomainEventBase
{
  public int SessionId { get; init; } = sessionId;
  public HandlingMode sessionMode { get; init; } = sessionMode;
}
