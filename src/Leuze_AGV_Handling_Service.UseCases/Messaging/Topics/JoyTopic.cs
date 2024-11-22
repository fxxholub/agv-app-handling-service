using MediatR;

namespace Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;

public record JoyTopic(float X, float Y, float W) : INotification;