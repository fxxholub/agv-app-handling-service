using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;
public record AgvModeTopic(string data) : INotification;
