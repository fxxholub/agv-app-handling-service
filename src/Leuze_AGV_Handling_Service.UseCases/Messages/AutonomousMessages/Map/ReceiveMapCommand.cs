using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.AutonomousMessages.Map;

public record ReceiveMapCommand(string Message) : ICommand<Result>;