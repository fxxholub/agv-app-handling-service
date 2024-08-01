using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Start;

public class StartSessionHandler(IStartSessionService startSessionService)
  : ICommandHandler<StartSessionCommand, Result>
{
  public async Task<Result> Handle(StartSessionCommand request, CancellationToken cancellationToken)
  {
    return await startSessionService.StartSession(request.SessionId);
  }
}
