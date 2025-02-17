using Ardalis.SharedKernel;

namespace Handling_Service.Core.Session.SessionAggregate;

/// <summary>
/// Describes runnable Bash command.
/// </summary>
/// <param name="command"></param>
public class ProcessCommand(string command) : EntityBase
{
    public string Command { get; private set; } = command;
    public int? ProcessId { get; set; }
}