using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface IProcessMonitorService
{
    public Task<string> StartProcess(Process process);
    public Task<bool> CheckProcess(Process process);
    public Task KillProcess(Process process);
}
