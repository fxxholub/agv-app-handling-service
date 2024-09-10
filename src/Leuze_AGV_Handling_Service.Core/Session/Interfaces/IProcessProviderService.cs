using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface IProcessProviderService
{
    public IEnumerable<Process> GetProcesses(HandlingMode handlingMode);
}
