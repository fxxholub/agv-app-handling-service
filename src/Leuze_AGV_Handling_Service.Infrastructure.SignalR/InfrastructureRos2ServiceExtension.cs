using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR;

/// <summary>
/// Registers Ros2 nodes with DI.
/// </summary>
public static class InfrastructureSignalRServiceExtension
{
  public static IServiceCollection AddInfrastructureRos2Services(
    this IServiceCollection services,
    ConfigurationManager config
    )
  {
    services.AddSwaggerGen(options =>                // Swagger SignalR
    {
      options.AddSignalRSwaggerGen();
    });
    
    return services;
  }
}
