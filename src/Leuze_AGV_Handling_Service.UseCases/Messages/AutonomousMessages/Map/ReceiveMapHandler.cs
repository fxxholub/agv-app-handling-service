using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Messages.AutonomousMessages.Joy;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.AutonomousMessages.Map;

public class ReceiveMapHandler(IAutonomousMessageNotifier notifier) 
    : ICommandHandler<ReceiveMapCommand, Result>
{
    public async Task<Result> Handle(ReceiveMapCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("get handled");
        return await notifier.NotifyReceiveMap(request.Message);
    }
}