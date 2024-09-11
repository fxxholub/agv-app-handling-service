using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Create;

public record CreateSessionCommand(
  HandlingMode HandlingMode,
  bool MappingEnabled,
  string? InputMapRef,
  string? OutputMapRef,
  string? OutputMapName
  ) : 
  Ardalis.SharedKernel.ICommand<Result<int>>;
