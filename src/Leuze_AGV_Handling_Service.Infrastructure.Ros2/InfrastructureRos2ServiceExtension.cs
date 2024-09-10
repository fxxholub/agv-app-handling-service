using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2;

/// <summary>
/// Registers Ros2 nodes with DI.
/// </summary>
public static class InfrastructureRos2ServiceExtension
{
  public static IServiceCollection AddInfrastructureRos2Services(
    this IServiceCollection services,
    ConfigurationManager config
    )
  {  
    services.AddHostedService<AutonomousSubscriber>();
    services.AddHostedService<ManualSubscriber>();
    services.AddSingleton<IAutonomousMessageSender, AutonomousPublisher>();
    services.AddSingleton<IManualMessageSender, ManualPublisher>();

    return services;
  }
}
