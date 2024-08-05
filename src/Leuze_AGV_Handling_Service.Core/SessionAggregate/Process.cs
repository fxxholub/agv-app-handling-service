using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Exceptions;
using Leuze_AGV_Handling_Service.Core.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate;

public class Process(
  string name,
  string hostName,
  string hostAddr,
  string userName,
  int? sessionId
  ) : EntityBase
{
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);
    
    public string HostName { get; private set; } = Guard.Against.NullOrEmpty(hostName);
    public string HostAddr { get; private set; } = Guard.Against.NullOrEmpty(hostAddr);
    public string UserName { get; private set; } = Guard.Against.NullOrEmpty(userName);
    public IEnumerable<string> Commands { get; private set; } = [];
    public int? SessionId { get; set; }  = sessionId;

    public string Pid { get; private set; } = string.Empty;

    public ProcessState State { get; private set; } = ProcessState.None;
    
    public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

    public void AddCommands(IEnumerable<string> commands)
    {
      Guard.Against.NullOrEmpty(commands);
      Commands = commands;
    }

    public async Task StartAsync(IProcessHandlerService processHandlerService)
    {
      if (State is ProcessState.Killed or ProcessState.Started)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Start while in {State.ToString()}.");
      }
      
      Pid = await processHandlerService.StartProcess(this);
      
      State = ProcessState.Started;
    }

    public async Task<bool> CheckAsync(IProcessHandlerService processHandlerService)
    {
      if (State is ProcessState.None or ProcessState.Killed)
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
    
    public async Task KillAsync(IProcessHandlerService processHandlerService)
    {
      if (State is ProcessState.None or ProcessState.Killed)
      {
        throw new ProcessInvalidOperationException(
          $"Invalid Process operation, cannot Kill while in {State.ToString()}.");
      }

      await processHandlerService.KillProcess(this);
    }

    // protected override IEnumerable<object> GetEqualityComponents()
    // {
    //   return new[] { Name, HostAddr, UserName, Pid };
    // }
}
