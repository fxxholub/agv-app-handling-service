using MediatR;

namespace Handling_Service.UseCases.Session.Notifications.Events;

/// <summary>
/// Notification for a bad session check.
/// </summary>
public record BadSessionCheckEvent(int SessionId) : INotification;