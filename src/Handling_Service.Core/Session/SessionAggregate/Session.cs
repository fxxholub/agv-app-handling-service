using Ardalis.SharedKernel;
using Handling_Service.Core.Session.Exceptions;
using Handling_Service.Core.Session.Interfaces;

namespace Handling_Service.Core.Session.SessionAggregate;

/// <summary>
/// Describes user handling session with its settings, processes and handling mode.
/// </summary>
/// <param name="handlingMode"></param>
public class Session(
  HandlingMode handlingMode,
  Lifespan lifespan,
  bool mapping
  ) : EntityBase, IAggregateRoot
{
  public HandlingMode HandlingMode { get; private set; } = handlingMode;
  
  public Lifespan Lifespan { get; private set; } = lifespan;
  
  public bool Mapping { get; private set; } = mapping;
  public SessionState State { get; private set; } = SessionState.None;
  
  private readonly List<Action> _actions = new List<Action>();
  public IEnumerable<Action> Actions => _actions.AsReadOnly();
  
  private readonly List<Process> _processes = new List<Process>();
  public IEnumerable<Process> Processes => _processes.AsReadOnly();
  
  public string? ErrorReason { get; set; }
  
  public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

  /// <summary>
  /// Add process instance to the session (only if process state is None).
  /// </summary>
  /// <param name="process"></param>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public void AddProcess(Process process)
  {
    if (process.State is not ProcessState.None)
    {
      throw new SessionInvalidOperationException(
        $"Invalid Session operation, cannot Add process which is in other state than {ProcessState.None.ToString()}.");
    }
    
    process.SessionId = Id;
    _processes.Add(process);
  }

  /// <summary>
  /// Starts a session`s underlying processes, checks if they started - updates states based on that.
  /// </summary>
  /// <param name="processMonitorFactory"></param>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task StartAsync(IProcessMonitorServiceFactory processMonitorFactory)
  {
    _actions.Add(new Action(ActionCommand.Start, Id));
    
    bool allGood = true;
    foreach (var process in _processes)
    {
      if (process.State is not ProcessState.Started)
      {
        var result = await process.StartAsync(processMonitorFactory);
        if (!result)
        {
          allGood = false;
          break;
        }
      }
    }

    if (allGood)
    {
      State = SessionState.Started;
      ErrorReason = null;
    }
    else
    {
      State = SessionState.Err;
      ErrorReason = "Some process/es did not start during session start.";
    }
  }

  /// <summary>
  /// Checks if underlying processes are alive, updates their state and session`s state based on that.
  /// </summary>
  /// <param name="processMonitorFactory"></param>
  /// <returns></returns>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task<bool> CheckAsync(IProcessMonitorServiceFactory processMonitorFactory)
  {
    if (State is SessionState.None)
    {
      throw new SessionInvalidOperationException(
        $"Invalid Session operation, cannot Check while in {State.ToString()}.");
    }

    bool allGood = true;
    foreach (var process in _processes)
    {
      if (!await process.CheckAsync(processMonitorFactory)) allGood = false;
    }

    if (!allGood)
    {
      State = SessionState.Err;
      ErrorReason = "Some process/es errored out when session checked.";
    }

    return allGood;
  }

  /// <summary>
  /// Ends session - kills all underlying processes.
  /// </summary>
  /// <param name="processMonitorFactory"></param>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task EndAsync(IProcessMonitorServiceFactory processMonitorFactory)
  {
    _actions.Add(new Action(ActionCommand.End, Id));
    
    if (State is SessionState.None)
    {
      State = SessionState.Ended;
      return;
    }
    
    foreach (var process in _processes)
    {
      await process.KillAsync(processMonitorFactory);
    }

    State = SessionState.Ended;
  }
}
