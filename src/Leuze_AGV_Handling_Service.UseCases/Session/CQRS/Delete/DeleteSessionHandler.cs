using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Delete;

/// <summary>
/// Deletes Session.
/// </summary>
/// <param name="sessionManager"></param>
public class DeleteSessionHandler(ISessionManagerService sessionManager)
  : ICommandHandler<DeleteSessionCommand, Result>
{
  public async Task<Result> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
  {
    return await sessionManager.EndAndDeleteSession(request.SessionId);
  }
}
