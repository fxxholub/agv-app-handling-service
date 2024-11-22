namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public interface IAutonomousHandlingHub
{
    Task ReceiveSessionUnexpectedEnd(string errorMessage);
}