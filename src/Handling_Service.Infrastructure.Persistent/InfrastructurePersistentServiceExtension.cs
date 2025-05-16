using Ardalis.SharedKernel;
using Handling_Service.Infrastructure.Persistent.InMemoryDb.Queries;
using Handling_Service.UseCases.Session.CQRS.CRUD.List;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Handling_Service.Infrastructure.Persistent;

/// <summary>
/// Registers Databases services with DI.
/// </summary>
public static class InfrastructurePersistentServiceExtension
{
  public static IServiceCollection AddInfrastructurePersistentServices(
    this IServiceCollection services,
    ConfigurationManager config
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

    return services;
  }
}
