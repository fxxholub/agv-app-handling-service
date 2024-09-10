using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.UseCases.Session;

public record ProcessDTO(
  string Name,
  string? HostName,
  string? HostAddr,
  string? UserName,
  int? SessionId,
  string Pid,
  ProcessState State,
  DateTimeOffset CreatedDate
);
