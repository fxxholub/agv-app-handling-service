using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record CmdVel(float X, float Y, float W) : INotification;