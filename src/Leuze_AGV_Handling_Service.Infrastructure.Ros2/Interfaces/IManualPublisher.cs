namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;

public interface IManualPublisher
{
    public Task PublishAgvModeManualTopic();
    public Task PublishJoyTopic(float x, float y, float w);
}