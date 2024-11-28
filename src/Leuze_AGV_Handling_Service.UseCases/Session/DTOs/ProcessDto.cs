using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

public record ProcessDto(
  string Name,
  string? HostName,
  int? SessionId,
  string? ErrorReason,
  string Pid,
  ProcessState State,
  DateTimeOffset CreatedDate
);
