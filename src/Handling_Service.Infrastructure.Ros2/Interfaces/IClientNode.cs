using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Core.Ros2.Interfaces.Nav2;
using LoadMapServiceRequest = Handling_Service.Core.Ros2.Interfaces.Nav2.LoadMapServiceRequest;

namespace Handling_Service.Infrastructure.Ros2.Interfaces;

public interface IClientNode
{
    public Task<Result<bool>> SaveMap(SaveMapServiceRequest data);
    public Task<Result<OccupancyGrid>> LoadMap(LoadMapServiceRequest data);
    public Task<Result<OccupancyGrid>> GetMap();
    
}