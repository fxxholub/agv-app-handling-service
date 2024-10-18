using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Ros2CommonMessages.Builtin;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.HighPerformance.Helpers;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 publisher of manual messages.
/// </summary>
public class ManualPublisher : IManualMessageSender
{
    private readonly ILogger<ManualPublisher> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Sensor.Joy> _joyPublisher;
    public ManualPublisher(IServiceProvider serviceProvider, ILogger<ManualPublisher> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 handling_service_manual_pub node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_manual_pub");

        _joyPublisher = node.CreatePublisher<Ros2CommonMessages.Sensor.Joy>("/handling_manual_joy");
    }

    public async Task SendJoy(JoyDto message)
    {
        var now = DateTime.UtcNow;
        var seconds = (int)(now - DateTime.UnixEpoch).TotalSeconds; // Seconds since the Unix epoch
        var nanoseconds = (uint)(now.Ticks % TimeSpan.TicksPerSecond * 100); 
        var timestamp = new Ros2CommonMessages.Builtin.Time(seconds, nanoseconds);
        
        var header = new Ros2CommonMessages.Std.Header(timestamp);

        var msg = new Ros2CommonMessages.Sensor.Joy
        {
            Header = header,
            Axes = [message.X, message.Y, message.W],
            Buttons = []
        };
        
        await _joyPublisher.PublishAsync(msg);
    }
}