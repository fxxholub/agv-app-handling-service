using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav;

namespace Handling_Service.Infrastructure.Ros2.Interfaces;

public interface IPublisherNode
{
    public Task PublishAgvMode(string mode);
    public Task PublishCmdVel(Twist data);
    
    public Task PublishPath(string path);
    
    public Task PublishDriving(bool driving);
    
    public Task PublishMap(OccupancyGrid map);
    
    public Task PublishInitialPose(PoseWithCovarianceStamped pose);
    
    public Task PublishGoalPose(PoseStamped pose);
}