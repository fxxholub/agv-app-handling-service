namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface IProcessService
{
    Task<string> StartProcess(IEnumerable<string> commands);
    Task<bool> CheckProcess(string pid);
    Task KillProcess(string pid);
    
}
