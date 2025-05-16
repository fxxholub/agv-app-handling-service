using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.Infrastructure.ProcessServices;

public class FakeProcessMonitorService: IProcessMonitorService
{
    public async Task<string> StartProcess(Process process)
    {
        await Task.Delay(500);
        return "1234";
    }

    public async Task<bool> CheckProcess(Process process)
    {
        await Task.Delay(500);
        return true;
    }

    public async Task KillProcess(Process process)
    {
        await Task.Delay(500);
    }
}
