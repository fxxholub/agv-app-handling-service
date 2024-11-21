using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public interface IAutonomousHandlingHub : IAutonomousMessageReceiver
{
    Task ReceiveSessionUnexpectedEnd(string errorMessage);
}