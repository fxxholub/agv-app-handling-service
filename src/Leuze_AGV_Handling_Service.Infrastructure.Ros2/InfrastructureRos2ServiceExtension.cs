using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Contexts;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

    services.AddScoped<IAutonomousMessageSendForwarder, AutonomousNodeSendForwarder>();
    services.AddScoped<IManualMessageSendForwarder, ManualNodeSendForwarder>();

    return services;
  }
}
