using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.AutonomousMessages.Joy;

public record SendJoyCommand(string Message) : ICommand<Result>;