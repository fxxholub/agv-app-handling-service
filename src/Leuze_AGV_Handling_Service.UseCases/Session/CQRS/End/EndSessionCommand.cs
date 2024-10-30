using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.End;

public record EndSessionCommand(int SessionId) : ICommand<Result>;
