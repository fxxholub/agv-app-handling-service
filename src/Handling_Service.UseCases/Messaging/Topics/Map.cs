using Handling_Service.UseCases.Messaging.DTOs;
using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record Map(MapDto map) : INotification;