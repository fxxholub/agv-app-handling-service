using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

public record SessionDTO(
  int Id,

  HandlingMode HandlingMode,
  bool MappingEnabled,
  string? InputMapRef,
  string? OutputMapRef,
  string? OutputMapName,
  string? ErrorReason,

  SessionState State,
  List<ProcessDTO> Processes,
  DateTimeOffset CreatedDate
);
