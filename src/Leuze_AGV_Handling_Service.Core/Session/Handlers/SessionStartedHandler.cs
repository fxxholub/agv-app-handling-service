using Ardalis.GuardClauses;
using Leuze_AGV_Handling_Service.Core.Session.Events;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using MediatR;

namespace Leuze_AGV_Handling_Service.Core.Session.Handlers;

/// <summary>
/// Handles session started event. Starts session Watchdog, to periodically check its processes.
/// </summary>
/// <param name="watchdog"></param>
public class SessionStartedHandler(ISessionWatchdogService watchdog) : INotificationHandler<SessionStartedEvent>
{
    public Task Handle(SessionStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        Guard.Against.Null(domainEvent, nameof(domainEvent));

        return watchdog.StartWatching(domainEvent.SessionId);
    }
}