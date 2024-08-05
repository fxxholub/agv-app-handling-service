using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Exceptions;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate;

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

  public async Task StartAsync(IProcessHandlerService processHandlerService)
  {
    if (State is not SessionState.None)
    {
      throw new SessionInvalidOperationException(
        $"Invalid Session operation, cannot Start from None while in {State.ToString()}.");
    }

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

  public async Task<bool> CheckAsync(IProcessHandlerService processHandlerService)
  {
    if (State is SessionState.None or SessionState.Ended)
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

  public async Task EndAsync(IProcessHandlerService processHandlerService)
  {
    if (State is SessionState.Ended)
    {
      throw new SessionInvalidOperationException(
        $"Invalid Session operation, cannot End while in {State.ToString()}.");
    }

    foreach (var process in _processes)
    {
      await process.KillAsync(processHandlerService);
    }
  }
}
