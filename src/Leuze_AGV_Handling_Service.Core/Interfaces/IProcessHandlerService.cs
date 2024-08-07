using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface IProcessHandlerService
{
    public Task<string> StartProcess(Process process);
    public Task<bool> CheckProcess(Process process);
    public Task KillProcess(Process process);
}
