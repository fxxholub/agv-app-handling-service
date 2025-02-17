using Handling_Service.UseCases.Messaging.DTOs;

namespace Handling_Service.Infrastructure.Ros2.Interfaces;

public interface ISubscriber
{
    public Task SubscribeMap(MapDto map);
}