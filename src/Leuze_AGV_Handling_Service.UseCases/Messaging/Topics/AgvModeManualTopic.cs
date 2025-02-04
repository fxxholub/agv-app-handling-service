using MediatR;

namespace Leuze_AGV_Handling_Service.UseCases.Messaging.Topics;

public record AgvModeManualTopic() : INotification;