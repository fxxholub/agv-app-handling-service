namespace Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;

/// <summary>
/// Pose DTO
/// </summary>
/// <param name="Position"> tuple representing vector3 (x, y, z)</param>
/// <param name="Orientation">tuple representing quaternion (x, y, z, w)</param>
public record PoseDto(
    (double X, double Y, double Z) Position,
    (double X, double Y, double Z, double W) Orientation
    );