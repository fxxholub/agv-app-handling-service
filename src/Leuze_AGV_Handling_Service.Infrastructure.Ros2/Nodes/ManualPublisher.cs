using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 publisher of manual messages.
/// </summary>
public class ManualPublisher : IManualPublisher
{
    private readonly ILogger<ManualPublisher> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _agvModePublisher;
    private readonly IRclPublisher<Ros2CommonMessages.Geometry.Twist> _joyPublisher;
    public ManualPublisher(IServiceProvider serviceProvider, ILogger<ManualPublisher> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 handling_service_manual_pub node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_manual_pub");

        // called joy for teleop_twist_joy package purposes
        _agvModePublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/AgvMode");
        _joyPublisher = node.CreatePublisher<Ros2CommonMessages.Geometry.Twist>("/cmd_vel");
    }
    
    public async Task PublishAgvModeTopic()
    {
        var msg = new Ros2CommonMessages.Std.String
        {
            Data = "manual",
        };
        
        await _agvModePublisher.PublishAsync(msg);
    }
    
    public async Task PublishJoyTopic(float x, float y, float w)
    {
        var msg = new Ros2CommonMessages.Geometry.Twist
        {
            // velocity limit for axis 0.1 m/s
            Linear = new Ros2CommonMessages.Geometry.Vector3(-x/1000, -y/1000, 0),
            Angular = new Ros2CommonMessages.Geometry.Vector3(0, 0, -w/1000)
        };

        await PublishAgvModeTopic();
        await _joyPublisher.PublishAsync(msg);
    }
}