using Ardalis.Result;
using Leuze_AGV_Handling_Service.UseCases.Messages.AutonomousMessages.Map;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

public class AutonomousNode : BackgroundService, IAutonomousMessageTransceiver
{
    private readonly IMediator _mediator;
    private readonly ILogger<AutonomousNode> _logger;

    private readonly IRclPublisher<Ros2CommonMessages.Std.String> _joyPublisher;
    private readonly IRclSubscription<Ros2CommonMessages.Std.String> _mapSubscriber;
    public AutonomousNode(IMediator mediator, ILogger<AutonomousNode> logger)
    {
        _mediator = mediator;
        
        _logger = logger;
        _logger.LogInformation($"Handling Ros2 node started.");
        Console.WriteLine("cw Handling Ros2 node started.");
        
        var context = new RclContext();
        var node = context.CreateNode("handling_service_autonomous");

        _joyPublisher = node.CreatePublisher<Ros2CommonMessages.Std.String>("/joy");
        _mapSubscriber = node.CreateSubscription<Ros2CommonMessages.Std.String>("/map");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // while (!cancellationToken.IsCancellationRequested)
        // {
        //     // if (_logger.IsEnabled(LogLevel.Information))
        //     // {
        //     // }
        //
        //     _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //     Console.WriteLine($"Worker running at: {DateTimeOffset.Now} cw");
        //     await Task.Delay(1000, cancellationToken);
        // }

        await foreach (var msg in _mapSubscriber.ReadAllAsync(cancellationToken))
        {
            Console.WriteLine($"cw subscribed {msg.Data}");
            await ReceiveMap(msg.Data);
        }
    }

    public async Task<Result> SendJoy(string message)
    {
        var msg = new Ros2CommonMessages.Std.String(message);
        try
        {
            await _joyPublisher.PublishAsync(msg);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result.Error();
        }

        return Result.Success();
    }

    public async Task ReceiveMap(string message)
    {
        var response = await _mediator.Send(new ReceiveMapCommand(message));
    }
}