using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface IProcessMonitorServiceFactory
{
    IProcessMonitorService GetService(DriverType driverType);
}