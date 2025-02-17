using Ardalis.GuardClauses;
using Handling_Service.Core.Session.Events;
using Handling_Service.Core.Session.Interfaces;
using MediatR;

namespace Handling_Service.Core.Session.Handlers;

/// <summary>
/// Handles session ended event. Stops session Watchdog.
/// </summary>
/// <param name="watchdog"></param>
public class SessionEndedHandler(ISessionWatchdogService watchdog) : INotificationHandler<SessionEndedEvent>
{
    public Task Handle(SessionEndedEvent domainEvent, CancellationToken cancellationToken)
    {
        Guard.Against.Null(domainEvent, nameof(domainEvent));

        return watchdog.StopWatching();
    }
}