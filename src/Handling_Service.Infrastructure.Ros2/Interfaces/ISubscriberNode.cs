using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav;

namespace Handling_Service.Infrastructure.Ros2.Interfaces;

public interface ISubscriberNode
{
    public Task SubscribeMap(OccupancyGrid data);
    
    public Task SubscribeRobotPose(PoseStamped data);
}