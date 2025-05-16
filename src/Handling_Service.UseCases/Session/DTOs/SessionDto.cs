using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.UseCases.Session.DTOs;

public record SessionDto(
  int Id,

  HandlingMode HandlingMode,
  string? ErrorReason,

  SessionState State,
  List<ActionDto> Actions,
  List<ProcessDto> Processes,
  DateTimeOffset CreatedDate
);
