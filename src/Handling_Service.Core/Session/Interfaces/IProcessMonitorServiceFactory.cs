using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.Core.Session.Interfaces;

public interface IProcessMonitorServiceFactory
{
    IProcessMonitorService GetService(DriverType driverType);
}