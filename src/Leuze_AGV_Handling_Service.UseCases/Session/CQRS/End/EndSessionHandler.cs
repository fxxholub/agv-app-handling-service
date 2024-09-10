using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.End;

public class EndSessionHandler(ISessionManagerService sessionManager)
  : ICommandHandler<EndSessionCommand, Result>
{
  public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
  {
    return await sessionManager.EndSession(request.SessionId);
  }
}
