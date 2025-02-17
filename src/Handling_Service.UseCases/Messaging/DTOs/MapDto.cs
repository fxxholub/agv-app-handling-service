namespace Handling_Service.UseCases.Messaging.DTOs;

public record MapDto(
    float Resolution,
    uint Width,
    uint Height,
    PoseDto Origin,
    sbyte[] Data
    );