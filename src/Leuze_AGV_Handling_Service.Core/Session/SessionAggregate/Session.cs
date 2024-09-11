using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Session.Exceptions;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

/// <summary>
/// Describes user handling session with its settings, processes and mode selected.
/// </summary>
/// <param name="handlingMode"></param>
/// <param name="mappingEnabled"></param>
/// <param name="inputMapRef"></param>
/// <param name="outputMapRef"></param>
/// <param name="outputMapName"></param>
public class Session(
  HandlingMode handlingMode,
  bool mappingEnabled,
  string? inputMapRef,
  string? outputMapRef,
  string? outputMapName
  ) : EntityBase, IAggregateRoot
{
  public HandlingMode HandlingMode { get; private set; } = handlingMode;
  public bool MappingEnabled { get; private set; } = mappingEnabled;
  public string? InputMapRef { get; set; } = inputMapRef;
  public string? OutputMapRef { get; set; } = outputMapRef;
  public string? OutputMapName { get; set; } = outputMapName;
  public SessionState State { get; private set; } = SessionState.None;
  
  private readonly List<Process> _processes = new List<Process>();
  public IEnumerable<Process> Processes => _processes.AsReadOnly();
  
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
    
    process.SessionId = this.Id;
    _processes.Add(process);
  }

  /// <summary>
  /// Starts a session`s underlying processes, checks if they started - updates states based on that.
  /// </summary>
  /// <param name="processHandlerService"></param>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task StartAsync(IProcessHandlerService processHandlerService)
  {
    foreach (var process in _processes)
    {
      if (process.State is not ProcessState.Started)
      {
        await process.StartAsync(processHandlerService);
      }
    }

    await Task.Delay(1000);

    bool allGood = true;
    foreach (var process in _processes)
    {
      if (!await process.CheckAsync(processHandlerService))
      {
        allGood = false;
      }
    }

    if (allGood)
    {
      State = SessionState.Started;
    }
    else
    {
      State = SessionState.Err;
    }
  }

  /// <summary>
  /// Checks if underlying processes are alive, updates their state and session`s state based on that.
  /// </summary>
  /// <param name="processHandlerService"></param>
  /// <returns></returns>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task<bool> CheckAsync(IProcessHandlerService processHandlerService)
  {
    if (State is SessionState.None)
    {
      throw new SessionInvalidOperationException(
        $"Invalid Session operation, cannot Check while in {State.ToString()}.");
    }

    bool allGood = true;
    foreach (var process in _processes)
    {
      if (!await process.CheckAsync(processHandlerService)) allGood = false;
    }

    if (!allGood) State = SessionState.Err;

    return allGood;
  }

  /// <summary>
  /// Ends session - kills all underlying processes.
  /// </summary>
  /// <param name="processHandlerService"></param>
  /// <exception cref="SessionInvalidOperationException"></exception>
  public async Task EndAsync(IProcessHandlerService processHandlerService)
  {
    if (State is SessionState.None)
    {
      throw new SessionInvalidOperationException(
        $"Invalid Session operation, cannot End while in {State.ToString()}.");
    }
    
    foreach (var process in _processes)
    {
      await process.KillAsync(processHandlerService);
    }

    State = SessionState.Ended;
  }
}
