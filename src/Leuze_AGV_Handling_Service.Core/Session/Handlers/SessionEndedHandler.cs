using Ardalis.GuardClauses;
using Leuze_AGV_Handling_Service.Core.Session.Events;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using MediatR;

namespace Leuze_AGV_Handling_Service.Core.Session.Handlers;

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