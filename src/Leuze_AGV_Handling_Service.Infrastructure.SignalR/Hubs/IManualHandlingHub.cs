using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;

public interface IManualHandlingHub : IManualMessageReceiver
{
    public Task ReceiveSession(SessionResponseModel response);
}