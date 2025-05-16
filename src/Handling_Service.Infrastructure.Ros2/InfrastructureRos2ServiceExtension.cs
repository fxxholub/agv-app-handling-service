using Handling_Service.Infrastructure.Ros2.Interfaces;
using Handling_Service.Infrastructure.Ros2.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Handling_Service.Infrastructure.Ros2;

/// <summary>
/// Registers Ros2 nodes with DI.
/// </summary>
public static class InfrastructureRos2ServiceExtension
{
    public static IServiceCollection AddInfrastructureRos2Services(
        this IServiceCollection services,
        ConfigurationManager config)
    {
        double cmdVelLinLim = GetScaleFromEnv("CMD_VEL_LIM_LIN", 1.0);
        double cmdVelAngLim = GetScaleFromEnv("CMD_VEL_LIM_ANG", 1.0);

        services.AddHostedService<SubscriberNode>();
        services.AddSingleton<IPublisherNode>(sp =>
            new PublisherNode(sp, sp.GetRequiredService<ILogger<PublisherNode>>(), cmdVelLinLim, cmdVelAngLim));
        services.AddSingleton<IClientNode, ClientNode>();

        return services;
    }

    private static double GetScaleFromEnv(string key, double defaultValue)
    {
        string? value = Environment.GetEnvironmentVariable(key);
        if (double.TryParse(value, out double result) && result >= 0)
        {
            return result;
        }
        return defaultValue;
    }
}