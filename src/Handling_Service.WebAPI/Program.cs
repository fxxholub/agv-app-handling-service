using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Ardalis.SharedKernel;
using Asp.Versioning;
using Handling_Service.Infrastructure;
using Handling_Service.Infrastructure.Persistent;
using Handling_Service.Infrastructure.Ros2;
using Handling_Service.Infrastructure.SignalR;
using Handling_Service.Infrastructure.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Handling_Service.WebAPI;

public static class Program
{
    private static WebApplicationBuilder _builder = null!;

    public static void Main(string[] args)
    {
        
        BuildServices(args);
        
        var app = _builder.Build();

        ConfigureApp(app);
        
        Log.Information("Handling Service starting");
        
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
        
        _builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        
        ConfigureAuthentication();
        
        ConfigureSerilog();                                       // Serilog

        ConfigureMediatR();                                       // MediatR

        _builder.Services.AddControllers();                       // REST controllers
        _builder.Services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
            });
        _builder.Services.Configure<HubOptions>(options =>
        {
            options.MaximumReceiveMessageSize = 1024 * 1024 * 100; // 100 MB message size limit
        });

        _builder.Services.AddEndpointsApiExplorer();              // Swagger
        _builder.Services.AddSwaggerGen(options =>                // Swagger SignalR
        {
            options.AddSignalRSwaggerGen(config =>
            {
                var signalRAssembly = Assembly.Load("Handling_Service.Infrastructure.SignalR");
                config.ScanAssembly(signalRAssembly);
            });
        });
        
        ConfigureApiVersioning();

        ConfigureInfrastructureServices();
        
    }

    private static void ConfigureAuthentication()
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? _builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
        {
            throw new Exception("JWT Key must be at least 32 characters long.");
        }

        _builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _builder.Configuration["Jwt:Issuer"],
                    ValidAudience = _builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.Request.Path.StartsWithSegments(HandlingHub.HubPath))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }

    private static void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_builder.Configuration) // load settings from appsettings.json
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        
        _builder.Host.UseSerilog();
    }

    private static void ConfigureMediatR()
    {
        var mediatRAssemblies = new[]
        {
            Assembly.Load("Handling_Service.Core"),                       // Core
            Assembly.Load("Handling_Service.UseCases"),                   // UseCases
            Assembly.Load("Handling_Service.Infrastructure"),             // Infrastructure
            Assembly.Load("Handling_Service.Infrastructure.Persistent"),  // Infrastructure.Persistent
            Assembly.Load("Handling_Service.Infrastructure.Ros2"),        // Infrastructure.Ros2
            Assembly.Load("Handling_Service.Infrastructure.SignalR")      // Infrastructure.SignalR
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
        app.UseCors("CorsPolicy");

        app.MapHub<HandlingHub>($"/api/v1/handling/signalr");
        
        app.UseWebSockets();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
        
        app.UseHttpsRedirection();
    }
}
