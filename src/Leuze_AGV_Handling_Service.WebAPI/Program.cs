using System.Reflection;
using Ardalis.SharedKernel;
using Asp.Versioning;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure;
using Leuze_AGV_Handling_Service.Infrastructure.Persistent;
using Leuze_AGV_Handling_Service.Infrastructure.Ros2;
using Leuze_AGV_Handling_Service.UseCases.Session.Create;
using Leuze_AGV_Handling_Service.WebAPI.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using ILogger = Serilog.ILogger;

public class Program
{
    private static ILogger _logger = null!;
    private static WebApplicationBuilder _builder = null!;

    public static void Main(string[] args)
    {
        ConfigureLogger();
        _logger.Information("Starting web host");

        BuildServices(args);

        var app = _builder.Build();

        ConfigureApp(app);

        app.Run();
    }

    private static void ConfigureLogger()
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
    }

    private static void BuildServices(string[] args)
    {
        _builder = WebApplication.CreateBuilder(args);

        _builder.Host.UseSerilog((_, config) =>
            config.ReadFrom.Configuration(_builder.Configuration));

        ConfigureMediatR();                                      // MediatR

        _builder.Services.AddControllers();                       // REST controllers
        _builder.Services.AddSignalR();                           // SignalR hubs

        _builder.Services.AddEndpointsApiExplorer();              // Swagger
        ConfigureApiVersioning();

        _builder.Services.AddSwaggerGen(options =>                // Swagger SignalR
        {
            options.AddSignalRSwaggerGen();
        });

        ConfigureInfrastructureServices();
    }

    private static void ConfigureMediatR()
    {
        var mediatRAssemblies = new[]
        {
            Assembly.GetAssembly(typeof(Session)),                // Core
            Assembly.GetAssembly(typeof(CreateSessionCommand))    // UseCases
        };
        _builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
        _builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        _builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
    }

    private static void ConfigureApiVersioning()
    {
        _builder.Services.AddApiVersioning(options =>            // API versioning
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
    }

    private static void ConfigureInfrastructureServices()
    {
        _builder.Services.AddInfrastructureServices(             // Infrastructure services - common
            _builder.Configuration
        );
        _builder.Services.AddInfrastructurePersistentServices(   // Infrastructure services - persistent
            _builder.Configuration
        );
        _builder.Services.AddInfrastructureRos2Services(         // Infrastructure services - ROS2
            _builder.Configuration
        );
    }

    private static void ConfigureApp(WebApplication app)
    {
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

        // // map SignalR stuff
        // app.MapPost("broadcast", async (string message, IHubContext<HandlingHub, IHandlingHub> context) => {
        //   await context.Clients.All.ReceiveMessage(message);
        //   return Results.NoContent();
        // });
        app.MapHub<HandlingHub>($"/api/v1/signalr/handling-hub");
    }
}
