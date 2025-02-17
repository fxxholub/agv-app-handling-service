using Ardalis.Result;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.Create;

/// <summary>
/// Creates Session. Entity repository creation operation.
/// </summary>
/// <param name="HandlingMode"></param>
/// <param name="Lifespan"></param>
public record CreateSessionCommand(
  HandlingMode HandlingMode,
  Lifespan Lifespan
  ) : 
  Ardalis.SharedKernel.ICommand<Result<int>>;
