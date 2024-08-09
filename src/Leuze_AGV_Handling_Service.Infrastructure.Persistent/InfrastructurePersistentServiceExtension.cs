using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Infrastructure.Persistent.InMemoryDb.Queries;
using Leuze_AGV_Handling_Service.UseCases.Session.List;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Infrastructure.Persistent;

/// <summary>
/// Registers Databases services with DI.
/// </summary>
public static class InfrastructurePersistentServiceExtension
{
  public static IServiceCollection AddInfrastructurePersistentServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger
    )
  {
    // string? connectionString = config.GetConnectionString("SqliteConnection");
    // Guard.Against.Null(connectionString);
    services.AddDbContext<InMemoryDb.AppDbContext>(options =>
      options
        .UseInMemoryDatabase("HandlingServiceInMemoryDb")
        // .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning)) // not valid for in memory db
      );

    services.AddScoped(typeof(IRepository<>), typeof(InMemoryDb.EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(InMemoryDb.EfRepository<>));
    
    ////// use cases stuff ////////
    services.AddScoped<IListSessionsQueryService, ListSessionsQueryService>();
    
    logger.LogInformation("Persistent services registered");

    return services;
  }
}
