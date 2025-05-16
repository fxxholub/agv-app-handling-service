using Handling_Service.Infrastructure.SignalR.Interfaces;
using Handling_Service.Infrastructure.SignalR.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Handling_Service.Infrastructure.SignalR;

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
    services.AddScoped<IPushHub, HubMessageForwarder>();
    services.AddScoped<IClientNotifier, HubMessageForwarder>();
    
    return services;
  }
}
