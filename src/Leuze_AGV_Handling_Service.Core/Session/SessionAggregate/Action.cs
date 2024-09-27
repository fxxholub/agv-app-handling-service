using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

public class Action(
    ActionCommand command,
    int sessionId
    ) : EntityBase
{
    public ActionCommand Command { get; set; } = command;
    public int SessionId { get; set; }  = sessionId;
    public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;
}