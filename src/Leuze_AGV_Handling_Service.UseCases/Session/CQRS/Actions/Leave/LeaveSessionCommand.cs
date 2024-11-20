using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Actions.Leave;

/// <summary>
/// Leaves Session. Releases connection ownership. To be used in OnDisconnected methods or for manual releasing.
/// </summary>
/// <param name="ConnectionId"></param>
public record LeaveSessionCommand(string ConnectionId) : ICommand<Result>;
