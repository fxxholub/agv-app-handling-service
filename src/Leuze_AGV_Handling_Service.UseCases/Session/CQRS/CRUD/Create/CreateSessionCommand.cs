using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Create;

public record CreateSessionCommand(
  HandlingMode HandlingMode,
  Lifespan Lifespan
  ) : 
  Ardalis.SharedKernel.ICommand<Result<int>>;
