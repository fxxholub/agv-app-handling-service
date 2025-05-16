namespace Handling_Service.Infrastructure.SignalR.Models.Ros2;

public record OccupancyGridModel(
    float Resolution,
    uint Width,
    uint Height,
    PoseModel Origin,
    sbyte[] Data
);