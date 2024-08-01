using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Start;

public record StartSessionCommand(int SessionId) : ICommand<Result>;
