using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

public class ProcessMonitorServiceFactory(IServiceProvider serviceProvider) : IProcessMonitorServiceFactory
{
    public IProcessMonitorService GetService(DriverType driverType)
    {
        return driverType switch
        {
            DriverType.Ssh => serviceProvider.GetRequiredService<SshProcessMonitorService>(),
            DriverType.Docker => serviceProvider.GetRequiredService<DockerProcessMonitorService>(),
            _ => throw new NotSupportedException($"DriverType '{driverType}' is not supported.")
        };
    }
}