using Handling_Service.Core.Ros2.Interfaces.Geometry;
using Handling_Service.Core.Ros2.Interfaces.Nav;
using Handling_Service.Core.Ros2.Interfaces.Std;
using Handling_Service.Infrastructure.Ros2.Interfaces;
using Microsoft.Extensions.Logging;
using Rcl;
using Rcl.Qos;
using String = Handling_Service.Core.Ros2.Interfaces.Std.String;

namespace Handling_Service.Infrastructure.Ros2.Nodes;

/// <summary>
/// Ros2 publisher of manual messages.
/// </summary>
public class PublisherNode : IPublisherNode
{
    private readonly ILogger<PublisherNode> _logger;
    private readonly double _cmdVelLimLin;
    private readonly double _cmdVelLimAng;

    private readonly IRclPublisher<String> _agvModePublisher;
    private readonly IRclPublisher<Twist> _cmdVelPublisher;
    private readonly IRclPublisher<String> _pathPublisher;
    private readonly IRclPublisher<Bool> _drivingPublisher;
    private readonly IRclPublisher<OccupancyGrid> _mapPublisher;
    private readonly IRclPublisher<PoseWithCovarianceStamped> _initialPosePublisher;
    private readonly IRclPublisher<PoseStamped> _goalPosePublisher;

    public PublisherNode(IServiceProvider serviceProvider, ILogger<PublisherNode> logger, double cmdVelLimLin = 1.0, double cmdVelLimAng = 1.0)
    {
        _logger = logger;
        _cmdVelLimLin = cmdVelLimLin;
        _cmdVelLimAng = cmdVelLimAng;
        _logger.LogInformation($"Handling Ros2 handling_service_manual_pub node started. LinearScale: {_cmdVelLimLin}, AngularScale: {_cmdVelLimAng}");

        var context = new RclContext();
        var node = context.CreateNode("handling_service_publisher");

        _agvModePublisher = node.CreatePublisher<String>("/AgvMode");
        _cmdVelPublisher = node.CreatePublisher<Twist>("/cmd_vel");
        _pathPublisher = node.CreatePublisher<String>("/path");
        _drivingPublisher = node.CreatePublisher<Bool>("/driving");
        var mapQos = new PublisherOptions(
            new QosProfile
            {
                Durability = DurabilityPolicy.TransientLocal,
                Depth = 1,
                Reliability = ReliabilityPolicy.Reliable
            }
        );
        _mapPublisher = node.CreatePublisher<OccupancyGrid>("/map", mapQos);
        _initialPosePublisher = node.CreatePublisher<PoseWithCovarianceStamped>("/initial_pose");
        _goalPosePublisher = node.CreatePublisher<PoseStamped>("/goal_pose");
    }

    public async Task PublishAgvMode(string mode)
    {
        var msg = new String
        {
            Data = mode
        };

        await _agvModePublisher.PublishAsync(msg);
    }

    public async Task PublishCmdVel(Twist data)
    {
        // Assuming TwistDto.Linear.X, .Y, and .Angular.Z are in percent (0 to 100)
        // Scale down by configuration and convert to ROS units
        var msg = new Twist
        {
            Linear = new Vector3(
                x: -data.Linear.X * _cmdVelLimLin / 100.0,
                y: -data.Linear.Y * _cmdVelLimLin / 100.0,
                z: 0),
            Angular = new Vector3(
                x: 0,
                y: 0,
                z: -data.Angular.Z * _cmdVelLimAng / 100.0)
        };

        await _cmdVelPublisher.PublishAsync(msg);
    }

    public async Task PublishPath(string path)
    {
        var msg = new String
        {
            Data = path
        };

        await _pathPublisher.PublishAsync(msg);
    }

    public async Task PublishDriving(bool driving)
    {
        var msg = new Bool
        {
            Data = driving
        };

        await _drivingPublisher.PublishAsync(msg);
    }
    
    public async Task PublishMap(OccupancyGrid map)
    {
        await _mapPublisher.PublishAsync(map);
    }
    
    public async Task PublishInitialPose(PoseWithCovarianceStamped pose)
    {
        await _initialPosePublisher.PublishAsync(pose);
    }
    
    public async Task PublishGoalPose(PoseStamped pose)
    {
        await _goalPosePublisher.PublishAsync(pose);
    }
}