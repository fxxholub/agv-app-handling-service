using Leuze_AGV_Handling_Service.Core.Exceptions;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.Services;
using Leuze_AGV_Handling_Service.Infrastructure.Messages;
using Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
    services.AddScoped<IStartSessionService, StartSessionService>();
    services.AddScoped<IEndSessionService, EndSessionService>();
    services.AddScoped<ICheckSessionService, CheckSessionService>();
    services.AddScoped<ICreateSessionService, CreateSessionService>();
    services.AddScoped<IDeleteSessionService, DeleteSessionService>();
    
    ////// infrastructure stuff ///////
    services.AddScoped<IAutonomousMessageChannel, AutonomousMessageChannel>();
    services.AddScoped<IManualMessageChannel, ManualMessageChannel>();
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
