using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;
public record AgvMode(string mode) : INotification;
