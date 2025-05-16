using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Models.Ros2;
using Handling_Service.UseCases.Messaging.Topics;
using MediatR;

namespace Handling_Service.Infrastructure.SignalR.Handlers;

public class MapHandler(IPushHub pushHub) : INotificationHandler<MapTopic>
{
    public async Task Handle(MapTopic message, CancellationToken cancellationToken)
    {

        var model = new OccupancyGridModel
        (
            message.data.Info.Resolution,
            message.data.Info.Width,
            message.data.Info.Height,
            new PoseModel(
                new PointModel(
                    message.data.Info.Origin.Position.X,
                    message.data.Info.Origin.Position.Y,
                    message.data.Info.Origin.Position.Z),
                new QuaternionModel(
                    message.data.Info.Origin.Orientation.X,
                    message.data.Info.Origin.Orientation.Y,
                    message.data.Info.Origin.Orientation.Z,
                    message.data.Info.Origin.Orientation.W
                )),
            message.data.Data
        );
        await pushHub.SubscribeMap(model);
    }
}