using Ardalis.SharedKernel;

namespace Handling_Service.Core.Session.Events;

public class SessionEndedEvent : DomainEventBase
{
    public int SessionId { get; set; }

    public SessionEndedEvent(int sessionId)
    {
        SessionId = sessionId;
    }
}