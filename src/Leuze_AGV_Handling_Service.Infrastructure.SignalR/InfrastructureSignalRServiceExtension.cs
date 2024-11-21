using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
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
    
    services.AddSingleton<IAutonomousMessageReceiver, AutonomousHubMessageForwarder>();
    services.AddSingleton<IManualMessageReceiver, ManualHubMessageForwarder>();
    services.AddScoped<IAutonomousClientNotifier, AutonomousHubMessageForwarder>();
    services.AddScoped<IManualClientNotifier, ManualHubMessageForwarder>();
    
    return services;
  }
}
