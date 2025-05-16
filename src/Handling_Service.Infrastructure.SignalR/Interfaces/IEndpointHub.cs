using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Infrastructure.SignalR.Models.Ros2;

namespace Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IEndpointHub
{
    public Task PublishCmdVel(float x, float y, float w);

    public Task PublishPath(string path);
    
    public Task PublishDriving(bool driving);
    
    public Task<bool> PublishAndSaveMap(OccupancyGridModel map);
    
    public Task<OccupancyGridModel> LoadMap();
    
    public Task<OccupancyGridModel> GetMap();

    public Task PublishInitialPose(PoseModel pose);
    
    public Task PublishGoalPose(PoseModel pose);
}