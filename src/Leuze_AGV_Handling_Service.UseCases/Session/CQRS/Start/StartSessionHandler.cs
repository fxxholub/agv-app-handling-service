using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Start;

/// <summary>
/// Starts Session.
/// </summary>
public class StartSessionHandler(ISessionManagerService sessionManager)
  : ICommandHandler<StartSessionCommand, Result>
{
  public async Task<Result> Handle(StartSessionCommand request, CancellationToken cancellationToken)
  {
    return await sessionManager.StartSession(request.SessionId);
  }
}
