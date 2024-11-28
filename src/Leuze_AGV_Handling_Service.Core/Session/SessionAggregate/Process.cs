using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Exceptions;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

/// <summary>
/// Describes a process, that can be started, checked and killed.
/// </summary>
/// <param name="name"></param>
/// <param name="hostName"></param>
/// <param name="hostAddr"></param>
/// <param name="userName"></param>
/// <param name="sessionId"></param>
public class Process(
  string name,
  DriverType driveType,
  string? hostName,
  string? hostAddr,
  string? userName,
  string? password,
  int? sessionId,
  string? privateKeyPath
  ) : EntityBase
{
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);

    public DriverType DriverType { get; private set; } = driveType;
    public string? HostName { get; private set; } = hostName;
    
    // Both SSH and Docker stuff
    public string? HostAddr { get; private set; } = hostAddr;
    public string? UserName { get; private set; } = userName;
    
    public string? Password { get; private set; } = password;
    
    // SSH based stuff
    public string? PrivateKeyPath { get; private set; } = privateKeyPath;
    private readonly List<ProcessCommand> _commands = new List<ProcessCommand>();
    public IEnumerable<ProcessCommand> Commands => _commands.AsReadOnly();
    
    // Docker based stuff
    public string? Image { get; private set; } // For Docker
    public string? Tag { get; private set; }   // For Docker
    public string? Platform { get; private set; } // For Docker
    
    public int? SessionId { get; set; }  = sessionId;

    public string Pid { get; private set; } = string.Empty;

    public ProcessState State { get; private set; } = ProcessState.None;
    
    public string? ErrorReason { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Batch add commands, that forms a process.
    /// </summary>
    /// <param name="commands"></param>
    public void AddCommands(IEnumerable<ProcessCommand> commands)
    {
      Guard.Against.NullOrEmpty(commands);
      _commands.AddRange(commands);
    }

    /// <summary>
    /// Starts process with given Process Handler, marks process with Pid if succeded.
    /// </summary>
    /// <param name="processMonitorFactory"></param>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task StartAsync(IProcessMonitorServiceFactory processMonitorFactory)
    {
      var processMonitorService = processMonitorFactory.GetService(DriverType);
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
        Pid = pid;
      }
    }

    /// <summary>
    /// Checks if process is alive with given Process Handler.
    /// </summary>
    /// <param name="processMonitorFactory"></param>
    /// <returns></returns>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task<bool> CheckAsync(IProcessMonitorServiceFactory processMonitorFactory)
    {
      var processMonitorService = processMonitorFactory.GetService(DriverType);
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
        ErrorReason = "Process check resulted in error.";
      }

      return isOk;
    }

    /// <summary>
    /// Kills process with given Process Handler.
    /// </summary>
    /// <param name="processMonitorFactory"></param>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task KillAsync(IProcessMonitorServiceFactory processMonitorFactory)
    {
      var processMonitorService = processMonitorFactory.GetService(DriverType);
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
