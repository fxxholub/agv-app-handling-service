using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.Persistent;

/// <summary>
/// Registers Ros2 services with DI.
/// </summary>
public static class InfrastructureRos2ServiceExtension
{
  public static IServiceCollection AddInfrastructureRos2Services(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger
    )
  {
    
    services.AddHostedService<HandlingNodeService>();
    
    logger.LogInformation($"Ros2 services registered");

    return services;
  }
}
