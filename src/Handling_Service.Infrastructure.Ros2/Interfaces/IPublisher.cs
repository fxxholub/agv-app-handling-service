using Handling_Service.UseCases.Messaging.DTOs;

namespace Handling_Service.Infrastructure.Ros2.Interfaces;

public interface IPublisher
{
    public Task PublishAgvMode(string mode);
    public Task PublishCmdVel(CmdVelDto data);
    
    public Task PublishPath(string path);
}