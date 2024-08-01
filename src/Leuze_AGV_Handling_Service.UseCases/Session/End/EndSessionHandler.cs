using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;

namespace Leuze_AGV_Handling_Service.UseCases.Session.End;

public class EndSessionHandler(IEndSessionService endSessionService)
  : ICommandHandler<EndSessionCommand, Result>
{
  public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
  {
    return await endSessionService.EndSession(request.SessionId);
  }
}
