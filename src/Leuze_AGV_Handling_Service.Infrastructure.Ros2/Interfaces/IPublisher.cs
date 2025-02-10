namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;

public interface IPublisher
{
    public Task PublishAgvMode(string mode);
    public Task PublishCmdVel(float x, float y, float w);
}