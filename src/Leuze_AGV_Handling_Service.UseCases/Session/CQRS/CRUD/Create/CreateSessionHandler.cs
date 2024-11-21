using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Create;

/// <summary>
/// Creates Session. Entity repository creation operation.
/// </summary>
/// <param name="createService"></param>
public class CreateSessionHandler(ICreateSessionService createService) 
  : ICommandHandler<CreateSessionCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
  {
    try
    {
      return await createService.CreateSession(
        request.HandlingMode,
        request.Lifespan
      );
    }
    catch
    {
      return Result.Error(new ErrorList(["Unknown error requesting create session."]));
    }
  }
}
