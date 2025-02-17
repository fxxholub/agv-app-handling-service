namespace Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IAutonomousClientNotifier
{
    public Task SessionUnexpectedEnd(string errorMessage);
}