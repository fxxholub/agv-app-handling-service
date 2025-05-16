using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record PathTopic(string data) : INotification;