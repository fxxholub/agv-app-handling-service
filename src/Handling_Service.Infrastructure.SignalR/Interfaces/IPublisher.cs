using Handling_Service.UseCases.Messaging.DTOs;

namespace Handling_Service.Infrastructure.SignalR.Interfaces;

public interface IPublisher
{
    public Task PublishCmdVel(float x, float y, float w);
}