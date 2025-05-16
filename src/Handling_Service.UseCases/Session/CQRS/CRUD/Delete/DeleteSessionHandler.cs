using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.Delete;

/// <summary>
/// Deletes Session. Entity repository deletion operation.
/// </summary>
/// <param name="deleteService"></param>
public class DeleteSessionHandler(IDeleteSessionService deleteService, ILogger<DeleteSessionHandler> logger)
  : ICommandHandler<DeleteSessionCommand, Result>
{
  public async Task<Result> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await deleteService.DeleteSession(request.SessionId);
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Session {request.SessionId} delete error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
