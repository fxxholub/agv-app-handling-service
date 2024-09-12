using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

public class BashCommand(string command) : EntityBase
{
    public string Command { get; private set; } = command;
    public int? ProcessId { get; set; }
}