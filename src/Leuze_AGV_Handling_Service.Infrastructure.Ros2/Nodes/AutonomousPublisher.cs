using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 publisher of autonomous messages.
/// </summary>
public class AutonomousPublisher : IAutonomousMessageSender
{
    private readonly ILogger<AutonomousPublisher> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _joyPublisher;
    public AutonomousPublisher(ILogger<AutonomousPublisher> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_autonomous_pub");

        _joyPublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/joy");
    }

    public async Task SendJoy(JoyDTO message)
    {
        var msg = new Ros2CommonMessages.Std.String(message.Something);
        await _joyPublisher.PublishAsync(msg);
    }
}