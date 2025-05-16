using Ardalis.Result;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.Create;

/// <summary>
/// Creates Session. Entity repository creation operation.
/// </summary>
/// <param name="HandlingMode"></param>
/// <param name="Lifespan"></param>
/// <param name="Mapping"></param>
public record CreateSessionCommand(
  HandlingMode HandlingMode,
  Lifespan Lifespan,
  bool Mapping
  ) : 
  Ardalis.SharedKernel.ICommand<Result<int>>;
