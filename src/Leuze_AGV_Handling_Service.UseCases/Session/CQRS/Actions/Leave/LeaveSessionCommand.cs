using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Leave;

public record LeaveSessionCommand(string ConnectionId) : ICommand<Result>;
