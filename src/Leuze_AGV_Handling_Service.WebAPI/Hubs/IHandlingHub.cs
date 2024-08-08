namespace Leuze_AGV_Handling_Service.WebAPI.Hubs;

public interface IHandlingHub
{
    Task ReceiveMessage(string message);

    Task GetSomeTopic();
}