using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record DrivingTopic(bool data) : INotification;