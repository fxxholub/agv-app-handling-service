using Ardalis.Result;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Contexts;

public class AutonomousMessageNotifierService(
    IHubContext<AutonomousHandlingHub, IAutonomousHandlingHub> hubContext,
    ILogger<AutonomousMessageNotifierService> logger)
    : IAutonomousMessageNotifier
{
    public async Task<Result> NotifyReceiveMap(string message)
    {
        try
        {
            await hubContext.Clients.All.ReceiveMap(message);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Result.Error();
        }

        return Result.Success();
    }
}