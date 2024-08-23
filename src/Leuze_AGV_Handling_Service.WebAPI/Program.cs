using System.Reflection;
using Ardalis.SharedKernel;
using Asp.Versioning;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure;
using Leuze_AGV_Handling_Service.Infrastructure.Persistent;
using Leuze_AGV_Handling_Service.Infrastructure.Ros2;
using Leuze_AGV_Handling_Service.Infrastructure.Ros2.Nodes;
using Leuze_AGV_Handling_Service.UseCases.Session.Create;
using Leuze_AGV_Handling_Service.WebAPI.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using Serilog.Extensions.Logging;

/**
 * Logger
 */
var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

/**
 * Services Builder
 */
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(                                    // Logger
  (_, config) => 
  config.ReadFrom.Configuration(builder.Configuration));
var microsoftLogger = new SerilogLoggerFactory(logger)
  .CreateLogger<Program>();

ConfigureMediatR();                                         // MediatR

builder.Services.AddControllers();                          // REST controllers
builder.Services.AddSignalR();                              // SignalR hubs

builder.Services.AddEndpointsApiExplorer();                 // https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(options =>                // API versioning
{
  options.DefaultApiVersion = new ApiVersion(1, 0);
  options.ReportApiVersions = true;
  options.AssumeDefaultVersionWhenUnspecified = true;
  options.ApiVersionReader = ApiVersionReader.Combine(
    new UrlSegmentApiVersionReader(),
    new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
  options.GroupNameFormat = "'v'V";
  options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(options =>                  // Swagger SignalR
{
  options.AddSignalRSwaggerGen();
});

builder.Services.AddInfrastructureServices(                   // Infrastructure services - common
  builder.Configuration, 
  microsoftLogger
  );
builder.Services.AddInfrastructurePersistentServices(         // Infrastructure services - persistent
  builder.Configuration, 
  microsoftLogger
);
// builder.Services.AddInfrastructureRos2Services(               // Infrastructure services - ROS2
//   builder.Configuration, 
//   microsoftLogger
// );
builder.Services.AddHostedService<HandlingNodeService>();

/**
 * App
 */
var app = builder.Build();

if (true/*app.Environment.IsDevelopment()*/)
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
  });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// // map SignalR stuffI
// app.MapPost("broadcast", async (string message, IHubContext<HandlingHub, IHandlingHub> context) => {
//   await context.Clients.All.ReceiveMessage(message);
//   return Results.NoContent();
// });
app.MapHub<HandlingHub>($"/api/v1/signalr/handling-hub");

app.Run();


void ConfigureMediatR()
{
  var mediatRAssemblies = new[]
  {
    Assembly.GetAssembly(typeof(Session)), // Core
    Assembly.GetAssembly(typeof(CreateSessionCommand)) // UseCases
  };
  builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
  builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
  builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
}


// Make the implicit Program.cs class public,
// so integration tests can reference the correct assembly for host building
public partial class Program
{
}
