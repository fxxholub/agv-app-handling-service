using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.UseCases.Session;

public record ProcessDTO(
  string Name,
  int SessionId,
  string Pid,
  ProcessState State,
  DateTimeOffset CreatedDate
);
