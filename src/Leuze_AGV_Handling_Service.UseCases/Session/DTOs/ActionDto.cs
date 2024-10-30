using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

public record ActionDto(
  ActionCommand Command,
  int SessionId,
  DateTimeOffset CreatedDate
);
