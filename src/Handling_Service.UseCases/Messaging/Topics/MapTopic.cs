using Handling_Service.Core.Ros2.Interfaces.Nav;
using MediatR;

namespace Handling_Service.UseCases.Messaging.Topics;

public record MapTopic(OccupancyGrid data) : INotification;