namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IAutonomousClientNotifier
{
    public Task ReceiveSessionUnexpectedEnd(string errorMessage);
}