using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Start;

public class StartSessionHandler()
  : ICommandHandler<StartSessionCommand, Result>
{
  public async Task<Result> Handle(StartSessionCommand request, CancellationToken cancellationToken)
  {
    // TODO
    // return await sessionManager.(request.SessionId);
    await Task.Delay(1);
    return Result.NotFound();
  }
}
