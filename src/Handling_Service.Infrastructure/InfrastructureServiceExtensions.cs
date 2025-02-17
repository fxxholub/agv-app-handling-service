using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.Services;
using Handling_Service.Infrastructure.ProcessServices;
using Handling_Service.Infrastructure.SessionServices;
using Handling_Service.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Handling_Service.Infrastructure;

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

    services.AddScoped<IProcessMonitorServiceFactory, ProcessMonitorServiceFactory>();
    
    services.AddScoped<SshProcessMonitorService>();
    services.AddScoped<DockerProcessMonitorService>();
    
    services.AddSingleton<IProcessProviderService, ProcessProviderService>();

    return services;
  }
}
