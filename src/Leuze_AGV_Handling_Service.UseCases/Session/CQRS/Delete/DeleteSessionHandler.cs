using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Delete;

public class DeleteSessionHandler(IDeleteSessionService deleteSessionService)
  : ICommandHandler<DeleteSessionCommand, Result>
{
  public async Task<Result> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
  {
    return await deleteSessionService.DeleteSession(request.SessionId);
  }
}
