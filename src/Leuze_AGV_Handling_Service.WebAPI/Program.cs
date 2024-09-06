using System.Reflection;
using Ardalis.SharedKernel;
using Asp.Versioning;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure;
using Leuze_AGV_Handling_Service.Infrastructure.Persistent;
using Leuze_AGV_Handling_Service.Infrastructure.Ros2;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR;
using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Hubs;
using Leuze_AGV_Handling_Service.UseCases.Session.Create;
using MediatR;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Leuze_AGV_Handling_Service.WebAPI;

public static class Program
{
    private static WebApplicationBuilder _builder = null!;

    public static void Main(string[] args)
    {
        
        BuildServices(args);
        
        var app = _builder.Build();

        ConfigureApp(app);
        
        Log.Information("Leuze AGV Handling Service starting");
        
        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void BuildServices(string[] args)
    {
        _builder = WebApplication.CreateBuilder(args);
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_builder.Configuration) // load settings from appsettings.json
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        
        _builder.Host.UseSerilog();

        ConfigureMediatR();                                      // MediatR

        _builder.Services.AddControllers();                       // REST controllers
        _builder.Services.AddSignalR();                           // SignalR hubs

        _builder.Services.AddEndpointsApiExplorer();              // Swagger
        _builder.Services.AddSwaggerGen(options =>                // Swagger SignalR
        {
            options.AddSignalRSwaggerGen();
        });
        ConfigureApiVersioning();

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
        _builder.Services.AddInfrastructureServices(             // Infrastructure services - core
            _builder.Configuration
        );
        _builder.Services.AddInfrastructurePersistentServices(   // Infrastructure services - persistent
            _builder.Configuration
        );
        _builder.Services.AddInfrastructureRos2Services(         // Infrastructure services - ROS2
            _builder.Configuration
        );
        _builder.Services.AddInfrastructureSignalRServices(      // Infrastructure services - SignalR
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

        // // map SignalR hubs as endpoints
        app.MapHub<AutonomousHandlingHub>($"/api/v1/signalr/autonomous");
    }
}
