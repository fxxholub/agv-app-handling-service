using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.Services;
using Leuze_AGV_Handling_Service.Infrastructure.Exceptions;
using Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;
using Leuze_AGV_Handling_Service.Infrastructure.SessionServices;
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
    services.AddSingleton<ISessionExecutorService, SessionExecutorService>();
    
    services.AddScoped<IStartSessionService, StartSessionService>();
    services.AddScoped<IEndSessionService, EndSessionService>();
    services.AddScoped<ICheckSessionService, CheckSessionService>();
    services.AddScoped<ICreateSessionService, CreateSessionService>();
    services.AddScoped<IDeleteSessionService, DeleteSessionService>();

    ////// infrastructure stuff ///////
    services.AddSingleton<SessionWatchdogService>();
    services.AddSingleton<ISessionWatchdogService>(sp => sp.GetRequiredService<SessionWatchdogService>());
    services.AddHostedService(sp => sp.GetRequiredService<SessionWatchdogService>());
    
    
    services.AddScoped<IProcessMonitorService, SshProcessMonitorService>();
    // services.AddScoped<IProcessHandlerService, FakeProcessHandlerService>();
    
    services.AddSingleton<IProcessProviderService>(provider => 
      new FileProcessProviderService(
        Environment.GetEnvironmentVariable("PROCESS_SCRIPTS_PATH_INNER") ?? throw new EnvironmentVariableNullException()
      ));
    // services.AddSingleton<IProcessProviderService, FakeProcessProviderService>();

    return services;
  }
}
