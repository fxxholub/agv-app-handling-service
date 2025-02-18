using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record Path(string path) : INotification;