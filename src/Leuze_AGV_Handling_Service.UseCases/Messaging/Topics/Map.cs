using Leuze_AGV_Handling_Service.UseCases.Messaging.DTOs;
using MediatR;

namespace Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;

public record Map(MapDto map) : INotification;