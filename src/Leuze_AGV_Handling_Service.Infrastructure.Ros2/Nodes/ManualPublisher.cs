using Leuze_AGV_Handling_Service.Core.Messages.DTOs;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

public class ManualPublisher : IManualMessageSender
{
    private readonly ILogger<ManualPublisher> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _joyPublisher;
    public ManualPublisher(IServiceProvider serviceProvider, ILogger<ManualPublisher> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_manual_pub");

        _joyPublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/joy");
    }

    public async Task SendJoy(JoyDTO message)
    {
        var msg = new Ros2CommonMessages.Std.String(message.Something);
        await _joyPublisher.PublishAsync(msg);
    }
}