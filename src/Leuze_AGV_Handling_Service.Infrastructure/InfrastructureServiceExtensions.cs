using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.Services;
using Leuze_AGV_Handling_Service.Infrastructure.InMemoryDb;
using Leuze_AGV_Handling_Service.Infrastructure.ProcessService;
using Leuze_AGV_Handling_Service.UseCases.Session.List;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger
    )
  {
    /////// persistence stuff ///////
    // string? connectionString = config.GetConnectionString("SqliteConnection");
    // Guard.Against.Null(connectionString);
    services.AddDbContext<InMemoryDb.AppDbContext>(options =>
      options
        .UseInMemoryDatabase("HandlingServiceInMemoryDb")
        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
      );

    services.AddScoped(typeof(IRepository<>), typeof(InMemoryDb.EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(InMemoryDb.EfRepository<>));
    
    ////// use cases stuff ////////
    // services.AddScoped<IListSessionsQueryService, ListSessionsQueryService>();
    
    ////// core stuff ////////
    services.AddScoped<IStartSessionService, StartSessionService>();
    services.AddScoped<IEndSessionService, EndSessionService>();
    services.AddScoped<ICheckSessionService, CheckSessionService>();
    services.AddScoped<ICreateSessionService, CreateSessionService>();
    services.AddScoped<IDeleteSessionService, DeleteSessionService>();
    
    ////// infrastructure stuff ///////
    services.AddScoped<IProcessService, SshProcessService>();
    
    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
