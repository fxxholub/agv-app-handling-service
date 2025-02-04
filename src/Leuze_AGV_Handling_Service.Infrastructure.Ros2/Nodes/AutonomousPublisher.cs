using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Interfaces;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 publisher of autonomous messages.
/// </summary>
public class AutonomousPublisher : IAutonomousPublisher
{
    private readonly ILogger<AutonomousPublisher> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _agvModePublisher;
    public AutonomousPublisher(ILogger<AutonomousPublisher> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 handling_service_autonomous_pub node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_autonomous_pub");

        _agvModePublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/AgvMode");
    }
    
    public async Task PublishAgvModeTopic()
    {
        await Task.Delay(1);
        // var msg = new Ros2CommonMessages.Std.String
        // {
        //     Data = "automatic",
        // };
        //
        // await _agvModePublisher.PublishAsync(msg);
    }
}