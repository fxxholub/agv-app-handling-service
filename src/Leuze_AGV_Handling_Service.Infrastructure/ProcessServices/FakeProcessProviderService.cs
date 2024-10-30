using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

public class FakeProcessProviderService: IProcessProviderService
{  
    public IEnumerable<Process> GetProcesses(HandlingMode handlingMode)
    {
        return new List<Process>([
            new Process(
            "process1",
            "host1",
            "172.17.0.1",
            "jholub",
            null,
            null
            ),
            new Process(
                "process2",
                "host3",
                "172.17.0.1",
                "jholub",
                null,
                null
            ),
        ]);
    }
}
