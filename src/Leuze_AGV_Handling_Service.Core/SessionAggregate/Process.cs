using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Exceptions;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate;

public class Process(IProcessService processService,string name, string sessionId, IEnumerable<string> commands): EntityBase
{
    private readonly IProcessService _processService = processService; 
  
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);

    public string SessionId { get; private set; }  = Guard.Against.NullOrEmpty(sessionId);

    public readonly IEnumerable<string> Commands = Guard.Against.NullOrEmpty(commands, nameof(commands));

    public string Pid { get; private set; } = string.Empty;

    public ProcessState State { get; private set; } = ProcessState.None;
    
    public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

    public async Task StartAsync()
    {
      if (State is ProcessState.Killed or ProcessState.Started)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Start while in {State.ToString()}.");
      }
      
      Pid = await _processService.StartProcess(Commands);
      
      State = ProcessState.Started;
    }

    public async Task<bool> CheckAsync()
    {
      if (State is ProcessState.None or ProcessState.Killed)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Check while in {State.ToString()}.");
      }

      if (State is ProcessState.Err) return false;
      
      bool isOk = await _processService.CheckProcess(Pid);

      if (!isOk)
      {
        State = ProcessState.Err;
      }

      return isOk;
    } 
    
    public async Task KillAsync()
    {
      if (State is ProcessState.None or ProcessState.Killed)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Kill while in {State.ToString()}.");
      }

      await _processService.KillProcess(Pid);
    }
}
