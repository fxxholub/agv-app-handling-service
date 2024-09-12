using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Exceptions;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

/// <summary>
/// Describes a process, that can be started, checked and killed.
/// Process can be handled localy, remotely or in other custom way - as IProcessHandlerService does.
/// </summary>
/// <param name="name"></param>
/// <param name="hostName"></param>
/// <param name="hostAddr"></param>
/// <param name="userName"></param>
/// <param name="sessionId"></param>
public class Process(
  string name,
  string? hostName,
  string? hostAddr,
  string? userName,
  int? sessionId,
  string? privateKeyPath
  ) : EntityBase
{
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);
    
    public string? HostName { get; private set; } = hostName;
    public string? HostAddr { get; private set; } = hostAddr;
    public string? UserName { get; private set; } = userName;
    
    public string? PrivateKeyPath { get; private set; } = privateKeyPath;

    private readonly List<string> _commands = new List<string>();
    public IEnumerable<string> Commands => _commands.AsReadOnly();
    public int? SessionId { get; set; }  = sessionId;

    public string Pid { get; private set; } = string.Empty;

    public ProcessState State { get; private set; } = ProcessState.None;
    
    public string? ErrorReason { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Batch add commands, that forms a process.
    /// </summary>
    /// <param name="commands"></param>
    public void AddCommands(IEnumerable<string> commands)
    {
      Guard.Against.NullOrEmpty(commands);
      _commands.AddRange(commands);
    }

    /// <summary>
    /// Starts process with given Process Handler, marks process with Pid if succeded.
    /// </summary>
    /// <param name="processMonitorService"></param>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task StartAsync(IProcessMonitorService processMonitorService)
    {
      var pid = await processMonitorService.StartProcess(this);

      if (string.IsNullOrEmpty(pid))
      {
        State = ProcessState.Err;
        ErrorReason = "Process did not start successfully.";
      }
      else
      {
        State = ProcessState.Started;
        ErrorReason = null;
      }
    }

    /// <summary>
    /// Checks if process is alive with given Process Handler.
    /// </summary>
    /// <param name="processMonitorService"></param>
    /// <returns></returns>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task<bool> CheckAsync(IProcessMonitorService processMonitorService)
    {
      if (State is ProcessState.None)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Check while in {State.ToString()}.");
      }

      if (State is ProcessState.Err) return false;
      
      bool isOk = await processMonitorService.CheckProcess(this);

      if (!isOk)
      {
        State = ProcessState.Err;
        ErrorReason = "Process errored out when checked.";
      }

      return isOk;
    } 
    
    /// <summary>
    /// Kills process with given Process Handler.
    /// </summary>
    /// <param name="processMonitorService"></param>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task KillAsync(IProcessMonitorService processMonitorService)
    {
      if (State is ProcessState.None)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Kill while in {State.ToString()}.");
      }
      await processMonitorService.KillProcess(this);

      if (State is not ProcessState.Err)
      {
        State = ProcessState.Killed;
      }
    }
}
