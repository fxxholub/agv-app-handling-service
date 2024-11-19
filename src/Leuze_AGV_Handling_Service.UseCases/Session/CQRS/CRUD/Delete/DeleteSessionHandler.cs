using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Delete;

/// <summary>
/// Deletes Session.
/// </summary>
/// <param name="sessionManager"></param>
public class DeleteSessionHandler(IDeleteSessionService deleteService)
  : ICommandHandler<DeleteSessionCommand, Result>
{
  public async Task<Result> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
  {
    return await deleteService.DeleteSession(request.SessionId);
  }
}
