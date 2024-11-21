namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IManualClientNotifier
{
    public Task ReceiveSessionUnexpectedEnd(string errorMessage);
}