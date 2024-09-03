using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;

/// <summary>
/// Event notifying system about faulty session check.
/// </summary>
/// <param name="sessionId"></param>
internal sealed class SessionFaultyEvent(int sessionId) : DomainEventBase
{
  public int SessionId { get; init; } = sessionId;
}
