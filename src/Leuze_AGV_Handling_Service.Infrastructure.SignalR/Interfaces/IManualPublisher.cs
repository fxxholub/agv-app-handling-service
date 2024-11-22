namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IManualPublisher
{
    public Task PublishJoyTopic(float x, float y, float w);
}