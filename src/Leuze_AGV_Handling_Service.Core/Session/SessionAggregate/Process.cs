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
  int? sessionId
  ) : EntityBase
{
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);
    
    public string? HostName { get; private set; } = hostName;
    public string? HostAddr { get; private set; } = hostAddr;
    public string? UserName { get; private set; } = userName;

    private readonly List<string> _commands = new List<string>();
    public IEnumerable<string> Commands => _commands.AsReadOnly();
    public int? SessionId { get; set; }  = sessionId;

    public string Pid { get; private set; } = string.Empty;

    public ProcessState State { get; private set; } = ProcessState.None;
    
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
    /// <param name="processHandlerService"></param>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task StartAsync(IProcessHandlerService processHandlerService)
    {
      Pid = await processHandlerService.StartProcess(this);
      
      State = ProcessState.Started;
    }

    /// <summary>
    /// Checks if process is alive with given Process Handler.
    /// </summary>
    /// <param name="processHandlerService"></param>
    /// <returns></returns>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task<bool> CheckAsync(IProcessHandlerService processHandlerService)
    {
      if (State is ProcessState.None)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Check while in {State.ToString()}.");
      }

      if (State is ProcessState.Err) return false;
      
      bool isOk = await processHandlerService.CheckProcess(this);

      if (!isOk)
      {
        State = ProcessState.Err;
      }

      return isOk;
    } 
    
    /// <summary>
    /// Kills process with given Process Handler.
    /// </summary>
    /// <param name="processHandlerService"></param>
    /// <exception cref="ProcessInvalidOperationException"></exception>
    public async Task KillAsync(IProcessHandlerService processHandlerService)
    {
      if (State is ProcessState.None)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Kill while in {State.ToString()}.");
      }
      await processHandlerService.KillProcess(this);
    }
}
