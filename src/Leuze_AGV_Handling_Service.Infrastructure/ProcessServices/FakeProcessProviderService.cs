using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

public class FakeProcessProviderService: IProcessProviderService
{  
    public IEnumerable<Process> GetProcesses(HandlingMode handlingMode)
    {
        return new List<Process>([
            new Process(
            "process1",
            "host1",
            "",
            "",
            null
            ),
            new Process(
                "process2",
                "host3",
                "",
                "",
                null
            ),
        ]);
    }
}
