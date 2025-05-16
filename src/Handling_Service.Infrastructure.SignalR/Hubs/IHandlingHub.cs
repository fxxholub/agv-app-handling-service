using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Models.Ros2;

namespace Handling_Service.Infrastructure.SignalR.Hubs;

public interface IHandlingHub
{
    Task SessionUnexpectedEnd(string errorMessage);
    
    public Task SubscribeMap(OccupancyGridModel data);
    
    public Task SubscribeRobotPose(PoseModel data);
}