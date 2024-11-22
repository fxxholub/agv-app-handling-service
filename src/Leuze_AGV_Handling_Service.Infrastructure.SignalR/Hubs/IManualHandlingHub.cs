namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public interface IManualHandlingHub
{
    Task ReceiveSessionUnexpectedEnd(string errorMessage);
}