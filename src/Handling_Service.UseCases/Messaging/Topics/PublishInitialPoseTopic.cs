using Handling_Service.Core.Ros2.Interfaces.Geometry;
using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record PublishInitialPoseTopic(PoseWithCovarianceStamped Data) : INotification;