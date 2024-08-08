using Rcl;
using Rcl.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;

public class HandlingNode
{
    private readonly RclContext _context;
    private readonly IRclNode _node;

    public HandlingNode(string[]? args)
    {
        _context = new RclContext(args ?? []);
        _node = _context.CreateNode("my_dummy_node");
        
        _node.Logger.LogInformation($"Leuze_AGV_Handling_Service ROS 2 node {_node.Name} started.");

        var scanSub = _node.CreateSubscription<Ros2CommonMessages.Std.String>("/scan1");
        
        
    }
    
    
}