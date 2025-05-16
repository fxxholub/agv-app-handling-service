using Ardalis.Result;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Interfaces;
using Microsoft.Extensions.Logging;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.Create;

/// <summary>
/// Creates Session. Entity repository creation operation.
/// </summary>
/// <param name="createService"></param>
public class CreateSessionHandler(ICreateSessionService createService, ILogger<CreateSessionHandler> logger) 
  : ICommandHandler<CreateSessionCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await createService.CreateSession(
        request.HandlingMode,
        request.Lifespan,
        request.Mapping
      );
    }
    catch (Exception ex)
    {
      logger.LogDebug(ex, $"Session create error.");
      return Result.Error(new ErrorList(["Unhandled exception.", ex.Message]));
    }
  }
}
