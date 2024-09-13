using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

/// <summary>
/// Describes runnable Bash command.
/// </summary>
/// <param name="command"></param>
public class BashCommand(string command) : EntityBase
{
    public string Command { get; private set; } = command;
    public int? ProcessId { get; set; }
}