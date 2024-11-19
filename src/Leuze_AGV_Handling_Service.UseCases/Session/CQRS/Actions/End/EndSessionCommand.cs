using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.End;

public record EndSessionCommand(int SessionId, string ConnectionId) : ICommand<Result>;
