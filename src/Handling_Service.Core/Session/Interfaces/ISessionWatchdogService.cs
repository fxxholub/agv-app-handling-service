namespace Handling_Service.Core.Session.Interfaces;

public interface ISessionWatchdogService
{
    public Task StartWatching(int sessionId);
    public Task StopWatching();
}