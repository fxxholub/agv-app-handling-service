using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Autonomous;
using Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;
using Leuze_AGV_Handling_Service.Core.Messages.Services;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.Services;
using Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leuze_AGV_Handling_Service.Infrastructure;

/// <summary>
/// Registers Core and own Infrastructure services with DI.
/// </summary>
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config
    )
  {
    
    ////// core stuff ////////
    services.AddSingleton<ISessionManagerService, SessionManagerService>();
    
    services.AddScoped<IStartSessionService, StartSessionService>();
    services.AddScoped<IEndSessionService, EndSessionService>();
    services.AddScoped<ICheckSessionService, CheckSessionService>();
    services.AddScoped<ICreateSessionService, CreateSessionService>();
    services.AddScoped<IDeleteSessionService, DeleteSessionService>();
    
    services.AddSingleton<IAutonomousMessageChannel, AutonomousMessageChannel>();
    services.AddSingleton<IManualMessageChannel, ManualMessageChannel>();
    
    ////// infrastructure stuff ///////
    services.AddScoped<IProcessHandlerService>(provider => 
      new SshProcessHandlerService(
        ".ssh/private_key"
        // Environment.GetEnvironmentVariable("SSH_PRIVATE_KEY_PATH") ?? throw new EnvironmentVariableNullException()
        ));
    // services.AddScoped<IProcessHandlerService, FakeProcessHandlerService>();
    
    // services.AddSingleton<IProcessProviderService>(provider => 
    //   new FileProcessProviderService(
    //     "ProcessScripts"
    //     // Environment.GetEnvironmentVariable("PROCESS_SCRIPTS_PATH") ?? throw new EnvironmentVariableNullException()
    //     ));
    services.AddSingleton<IProcessProviderService, FakeProcessProviderService>();

    return services;
  }
}
