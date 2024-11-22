using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Services;
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
    services.AddScoped<IAutonomousSubscriber, AutonomousHubMessageForwarder>();
    services.AddScoped<IManualSubscriber, ManualHubMessageForwarder>();
    services.AddScoped<IAutonomousClientNotifier, AutonomousHubMessageForwarder>();
    services.AddScoped<IManualClientNotifier, ManualHubMessageForwarder>();
    
    return services;
  }
}
