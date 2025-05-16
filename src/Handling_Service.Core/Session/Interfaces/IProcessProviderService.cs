using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.Core.Session.Interfaces;

public interface IProcessProviderService
{
    public IEnumerable<Process> GetProcesses(HandlingMode handlingMode);
}
