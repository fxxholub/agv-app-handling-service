using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.UseCases.Contributors.Create;
using Leuze_AGV_Handling_Service.Core.ContributorAggregate;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Create;

public class CreateSessionHandler(ICreateSessionService createSessionService) 
  : ICommandHandler<CreateSessionCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
  {
    return await createSessionService.CreateSession(
      request.HandlingMode,
      request.MappingEnabled,
      request.InputMapRef,
      request.OutputMapRef,
      request.OutputMapName
    );
  }
}
