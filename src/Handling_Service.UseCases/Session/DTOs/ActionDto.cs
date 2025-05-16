using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.UseCases.Session.DTOs;

public record ActionDto(
  ActionCommand Command,
  int SessionId,
  DateTimeOffset CreatedDate
);
