using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Infrastructure.ProcessService;
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
    // string? connectionString = config.GetConnectionString("SqliteConnection");
    // Guard.Against.Null(connectionString);
    // services.AddDbContext<AppDbContext>(options =>
    //  options.UseSqlite(connectionString));

    // services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    // services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    // services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
    // services.AddScoped<IDeleteContributorService, DeleteContributorService>();
    //
    // services.Configure<MailserverConfiguration>(config.GetSection("Mailserver"));

    services.AddScoped<IProcessService, SshProcessService>();
    
    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
