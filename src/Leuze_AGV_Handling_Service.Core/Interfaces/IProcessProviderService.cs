using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface IProcessProviderService
{
    public IEnumerable<Process> GetProcesses(HandlingMode handlingMode);
}
