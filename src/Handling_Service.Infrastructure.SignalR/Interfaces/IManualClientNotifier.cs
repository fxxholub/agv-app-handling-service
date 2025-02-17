namespace Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IManualClientNotifier
{
    public Task SessionUnexpectedEnd(string errorMessage);
}