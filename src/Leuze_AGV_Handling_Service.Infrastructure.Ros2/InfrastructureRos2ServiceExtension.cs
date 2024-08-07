using Ardalis.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.Ros2;

/// <summary>
/// Registers ROS2 services with DI.
/// </summary>
public static class InfrastructureRos2ServiceExtension
{
  public static IServiceCollection AddInfrastructureRos2Services(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger
    )
  {
    // services.AddScoped<IListSessionsQueryService, ListSessionsQueryService>();
    
    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
