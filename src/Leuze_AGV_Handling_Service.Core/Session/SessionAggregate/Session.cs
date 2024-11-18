using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Exceptions;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

/// <summary>
/// Describes user handling session with its settings, processes and handling mode.
/// </summary>
/// <param name="handlingMode"></param>
public class Session(
  HandlingMode handlingMode,
  Lifespan lifespan
  ) : EntityBase, IAggregateRoot
{
  public HandlingMode HandlingMode { get; private set; } = handlingMode;
  
  public Lifespan Lifespan { get; private set; } = lifespan;
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
        $"Invalid Session operation, cannot Add state which is in other state than {ProcessState.None.ToString()}.");
    }
    
    process.SessionId = Id;
    _processes.Add(process);
  }

  /// <summary>
  /// Starts a session`s underlying processes, checks if they started - updates states based on that.
  /// </summary>
  /// <param name="processMonitorService"></param>
  /// <param name="initCheckDelay"></param>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task StartAsync(IProcessMonitorService processMonitorService, int initCheckDelay)
  {
    _actions.Add(new Action(ActionCommand.Start, Id));
    
    foreach (var process in _processes)
    {
      if (process.State is not ProcessState.Started)
      {
        await process.StartAsync(processMonitorService);
      }
    }

    await Task.Delay(initCheckDelay);

    bool allGood = true;
    foreach (var process in _processes)
    {
      if (!await process.CheckAsync(processMonitorService))
      {
        allGood = false;
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
  /// <param name="processMonitorService"></param>
  /// <returns></returns>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task<bool> CheckAsync(IProcessMonitorService processMonitorService)
  {
    if (State is SessionState.None)
    {
      throw new SessionInvalidOperationException(
        $"Invalid Session operation, cannot Check while in {State.ToString()}.");
    }

    bool allGood = true;
    foreach (var process in _processes)
    {
      if (!await process.CheckAsync(processMonitorService)) allGood = false;
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
  /// <param name="processMonitorService"></param>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task EndAsync(IProcessMonitorService processMonitorService)
  {
    _actions.Add(new Action(ActionCommand.End, Id));
    
    if (State is SessionState.None)
    {
      State = SessionState.Ended;
      return;
    }
    
    foreach (var process in _processes)
    {
      await process.KillAsync(processMonitorService);
    }

    State = SessionState.Ended;
  }
}
