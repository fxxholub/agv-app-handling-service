using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;

/// <summary>
/// Event notifying system about ended session.
/// </summary>
/// <param name="sessionId"></param>
internal sealed class SessionEndedEvent(int sessionId, HandlingMode sessionMode) : DomainEventBase
{
  public int SessionId { get; init; } = sessionId;
  public HandlingMode sessionMode { get; init; } = sessionMode;
}
