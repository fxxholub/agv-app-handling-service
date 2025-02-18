using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.UseCases.Messaging.DTOs;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 publisher of manual messages.
/// </summary>
public class Publisher : IPublisher
{
    private readonly ILogger<Publisher> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _agvModePublisher;
    private readonly IRclPublisher<Ros2CommonMessages.Geometry.Twist> _cmdVelPublisher;
    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _pathPublisher;
    public Publisher(IServiceProvider serviceProvider, ILogger<Publisher> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 handling_service_manual_pub node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_manual_pub");
        
        _agvModePublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/AgvMode");
        _cmdVelPublisher = node.CreatePublisher<Ros2CommonMessages.Geometry.Twist>("/cmd_vel");
        _pathPublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/path");
    }
    
    public async Task PublishAgvMode(string mode)
    {
        var msg = new Ros2CommonMessages.Std.String
        {
            Data = mode
        };
        
        await _agvModePublisher.PublishAsync(msg);
    }
    
    public async Task PublishCmdVel(CmdVelDto data)
    {
        var msg = new Ros2CommonMessages.Geometry.Twist
        {
            // velocity limit for axis 0.1 m/s
            Linear = new Ros2CommonMessages.Geometry.Vector3(-data.X/1000, -data.Y/1000, 0),
            Angular = new Ros2CommonMessages.Geometry.Vector3(0, 0, -data.W/1000)
        };
        
        await _cmdVelPublisher.PublishAsync(msg);
    }
    
    public async Task PublishPath(string path)
    {
        var msg = new Ros2CommonMessages.Std.String()
        {
            Data = path
        };
        
        await _pathPublisher.PublishAsync(msg);
    }
}