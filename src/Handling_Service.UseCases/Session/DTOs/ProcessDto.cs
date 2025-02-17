using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.UseCases.Session.DTOs;

public record ProcessDto(
  string Name,
  string? HostName,
  int? SessionId,
  string? ErrorReason,
  string Pid,
  ProcessState State,
  DateTimeOffset CreatedDate
);
