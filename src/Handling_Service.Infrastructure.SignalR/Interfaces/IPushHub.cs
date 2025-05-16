using Handling_Service.Infrastructure.SignalR.Models.Ros2;

namespace Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IPushHub
{
    public Task SubscribeMap(OccupancyGridModel data);
    
    public Task SubscribeRobotPose(PoseModel data);
}