using Ardalis.Result;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Core.Ros2.Interfaces.Nav2;
using Handling_Service.Infrastructure.Ros2.Interfaces;
using Microsoft.Extensions.Logging;
using Rcl;
using LoadMapService = Handling_Service.Core.Ros2.Interfaces.Nav2.LoadMapService;
using LoadMapServiceRequest = Handling_Service.Core.Ros2.Interfaces.Nav2.LoadMapServiceRequest;
using LoadMapServiceResponse = Handling_Service.Core.Ros2.Interfaces.Nav2.LoadMapServiceResponse;

namespace Handling_Service.Infrastructure.Ros2.Nodes;

public class ClientNode : IClientNode
{
    private const int Timeout = 30000;
    
    private readonly ILogger<ClientNode> _logger;
    
    private readonly IRclClient<SaveMapServiceRequest, SaveMapServiceResponse> _saveMapClient;
    private readonly IRclClient<GetMapServiceRequest, GetMapServiceResponse> _getMapClient;
    private readonly IRclClient<LoadMapServiceRequest, LoadMapServiceResponse> _loadMapClient;
    
    public ClientNode(ILogger<ClientNode> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 handling_service_client node started.");

        var context = new RclContext();
        var node = context.CreateNode("handling_service_client");
        
        _saveMapClient = node.CreateClient<SaveMapService, SaveMapServiceRequest, SaveMapServiceResponse>("/map_saver/save_map");
        _getMapClient = node.CreateClient<GetMapService, GetMapServiceRequest, GetMapServiceResponse>("/map_server/map");
        _loadMapClient = node.CreateClient<LoadMapService, LoadMapServiceRequest, LoadMapServiceResponse>("/map_server/load_map");
    }

    public async Task<Result<bool>> SaveMap(SaveMapServiceRequest data)
    {
        using var cts = new CancellationTokenSource(Timeout);
        try
        {
            var response = await _saveMapClient.InvokeAsync(data, cts.Token);
            return new Result<bool>(response.Result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to save map with SaveMap in ROS: {ex.Message}");
            return Result.Error(new ErrorList(["Failed to save map in ROS."]));
        }
    }

    public async Task<Result<OccupancyGrid>> GetMap()
    {
        using var cts = new CancellationTokenSource(Timeout);
        try
        {
            var response = await _getMapClient.InvokeAsync(new GetMapServiceRequest(), cts.Token);
            return new Result<OccupancyGrid>(response.Map);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to retrieve map with GetMap in ROS: {ex.Message}");
            return Result.Error(new ErrorList(["Failed to get map in ROS."]));
        }
    }
    
    public async Task<Result<OccupancyGrid>> LoadMap(LoadMapServiceRequest data)
    {
        using var cts = new CancellationTokenSource(Timeout);
        try
        {
            var response = await _loadMapClient.InvokeAsync(data, cts.Token);
            if (response.Result != 0)
            {
                return Result.Error(new ErrorList([$"Failed to load map in ROS. LoadMap error code {response.Result}."]));
            }    
            return new Result<OccupancyGrid>(response.Map);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to retrieve map with LoadMap in ROS: {ex.Message}");
            return Result.Error(new ErrorList(["Error loading map in ROS."]));
        }
    }
}