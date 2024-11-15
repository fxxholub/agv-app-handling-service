namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface ISessionWatchdogService
{
    public Task StartWatching(int sessionId);
}