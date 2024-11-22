using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.Session.Events;

public class SessionEndedEvent : DomainEventBase
{
    public int SessionId { get; set; }

    public SessionEndedEvent(int sessionId)
    {
        SessionId = sessionId;
    }
}