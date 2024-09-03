using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.AutonomousMessages.Joy;

public class SendJoyHandler(IAutonomousMessageTransceiver transceiver) 
    : ICommandHandler<SendJoyCommand, Result>
{
    public async Task<Result> Handle(SendJoyCommand request, CancellationToken cancellationToken)
    {
        return await transceiver.SendJoy(request.Message);
    }
}