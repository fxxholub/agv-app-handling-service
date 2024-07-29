using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;

internal sealed class SessionCreatedEvent(int sessionId) : DomainEventBase
{
  public int SessionId { get; init; } = sessionId;
}
