namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;

public interface IAutonomousPublisher
{
    public Task PublishAgvModeTopic();
}