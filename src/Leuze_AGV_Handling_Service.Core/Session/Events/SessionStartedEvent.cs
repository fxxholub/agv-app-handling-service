using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.Session.Events;

public class SessionStartedEvent : DomainEventBase
{
    public int SessionId { get; set; }

    public SessionStartedEvent(int sessionId)
    {
        SessionId = sessionId;
    }
}