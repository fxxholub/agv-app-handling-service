using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rcl;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

public class HandlingNodeService : BackgroundService
{
    private readonly ILogger<HandlingNodeService> _logger;
    
    private readonly RclContext _context;
    private readonly IRclNode _node;
    
    private readonly IRclSubscription<Ros2CommonMessages.Std.String> _scanSubscriber;
    private int _sub_count = 0;

    public HandlingNodeService(ILogger<HandlingNodeService> logger)
    {
        _logger = logger;
    
        _logger.LogInformation($"Handling Ros2 node started.");
        Console.WriteLine("cw Handling Ros2 node started.");
        
        _context = new RclContext();
        _node = _context.CreateNode("my_dummy_node");
        
        _scanSubscriber = _node.CreateSubscription<Ros2CommonMessages.Std.String>("/chatter");
        
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

        await foreach (var msg in _scanSubscriber.ReadAllAsync(cancellationToken))
        {
            _logger.LogInformation($"subscribed {_sub_count}");
            _logger.LogInformation($"subscribed {msg.Data}");
            Console.WriteLine($"cw subscribed {_sub_count}");
            Console.WriteLine($"cw subscribed {msg.Data}");
            _sub_count++;
        }
    }
}