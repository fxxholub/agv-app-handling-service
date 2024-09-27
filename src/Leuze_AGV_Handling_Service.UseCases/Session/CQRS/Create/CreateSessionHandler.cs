using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Create;

/// <summary>
/// Creates Session.
/// </summary>
/// <param name="sessionManager"></param>
public class CreateSessionHandler(ISessionManagerService sessionManager) 
  : ICommandHandler<CreateSessionCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
  {
    var result = await sessionManager.CreateSession(
      request.HandlingMode,
      request.MappingEnabled,
      request.InputMapRef,
      request.OutputMapRef,
      request.OutputMapName
    );

    if (!result.IsSuccess)
    {
      return Result.Error();
    }

    return Result.Success(result.Value);
  }
}
