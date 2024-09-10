using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Contexts;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR;

/// <summary>
/// Registers Ros2 nodes with DI.
/// </summary>
public static class InfrastructureSignalRServiceExtension
{
  public static IServiceCollection AddInfrastructureSignalRServices(
    this IServiceCollection services,
    ConfigurationManager config
    )
  {
    
    services.AddScoped<IAutonomousMessageReceiver, AutonomousHubMessageForwarder>();
    services.AddScoped<IManualMessageReceiver, ManualHubMessageForwarder>();
    
    return services;
  }
}
