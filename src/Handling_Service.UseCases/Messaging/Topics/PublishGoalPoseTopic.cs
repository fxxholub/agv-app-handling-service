using Handling_Service.Core.Ros2.Interfaces.Geometry;
using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record PublishGoalPoseTopic(PoseStamped Data) : INotification;