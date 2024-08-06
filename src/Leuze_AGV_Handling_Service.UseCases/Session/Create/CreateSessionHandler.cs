using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.UseCases.Session.Create;

public class CreateSessionHandler(ICreateSessionService createSessionService) 
  : ICommandHandler<CreateSessionCommand, Result<SessionDTO>>
{
  public async Task<Result<SessionDTO>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
  {
    var entity = await createSessionService.CreateSession(
      request.HandlingMode,
      request.MappingEnabled,
      request.InputMapRef,
      request.OutputMapRef,
      request.OutputMapName
    );
    
    return new SessionDTO(
      entity.Value.Id,
      entity.Value.HandlingMode,
      entity.Value.MappingEnabled,
      entity.Value.InputMapRef ?? "",
      entity.Value.OutputMapRef ?? "",
      entity.Value.OutputMapName ?? "",
      entity.Value.State,
      entity.Value.Processes.Select(process => new ProcessDTO(
        process.Name,
        process.HostName,
        process.HostAddr,
        process.UserName,
        process.SessionId,
        process.Pid,
        process.State,
        process.CreatedDate
      )).ToList(),
      entity.Value.CreatedDate
    );
  }
}
