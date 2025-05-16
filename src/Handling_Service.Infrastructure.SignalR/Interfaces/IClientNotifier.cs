namespace Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IClientNotifier
{
    public Task SessionUnexpectedEnd(string errorMessage);
}