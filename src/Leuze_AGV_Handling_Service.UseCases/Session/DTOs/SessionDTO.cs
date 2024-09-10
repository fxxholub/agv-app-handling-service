using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.UseCases.Session;

public record SessionDTO(
  int Id,

  HandlingMode HandlingMode,
  bool MappingEnabled,
  string? InputMapRef,
  string? OutputMapRef,
  string? OutputMapName,

  SessionState State,
  List<ProcessDTO> Processes,
  DateTimeOffset CreatedDate
);
