using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Start;

public record StartSessionCommand(int SessionId, string ConnectionId) : ICommand<Result>;
