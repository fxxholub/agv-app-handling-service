using Leuze_AGV_Handling_Service.Core.Exceptions;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Renci.SshNet;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

public class FakeProcessHandlerService: IProcessHandlerService
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
