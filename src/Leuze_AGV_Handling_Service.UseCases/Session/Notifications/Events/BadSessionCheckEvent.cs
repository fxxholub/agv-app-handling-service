using MediatR;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Notifications.Events;

/// <summary>
/// Notification for a bad session check.
/// </summary>
public class BadSessionCheckEvent : INotification
{
    public int SessionId { get; }

    public BadSessionCheckEvent(int sessionId)
    {
        SessionId = sessionId;
    }
}