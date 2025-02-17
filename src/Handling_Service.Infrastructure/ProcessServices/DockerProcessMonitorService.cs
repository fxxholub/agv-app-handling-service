using Docker.DotNet;
using Docker.DotNet.Models;
using Handling_Service.Core.Session.Interfaces;
using Handling_Service.Core.Session.SessionAggregate;
using Microsoft.Extensions.Logging;

namespace Handling_Service.Infrastructure.ProcessServices;

/// <summary>
/// Handles process starting, checking and killing via Docker API.
/// </summary>
public class DockerProcessMonitorService(ILogger<DockerProcessMonitorService> logger): IProcessMonitorService
{
    /// <summary>
    /// Starts a containerized process via Docker API.
    /// </summary>
    /// <param name="process">The process to start.</param>
    /// <returns>The container ID if successful, empty string otherwise.</returns>
    public async Task<string> StartProcess(Process process)
    {
        ValidateProcessProperties(process);

        try 
        {
            DockerClient client = new DockerClientConfiguration(
                    new Uri(process.HostAddr!))
                .CreateClient();

            bool started = await client.Containers.StartContainerAsync(
                process.DockerContainerId,
                new ContainerStartParameters()
            );

            return "-";
        }
        catch
        {
            logger.LogWarning($"Error when checking process {process.Name} id {process.Id} under session {process.SessionId}.");
        }

        return string.Empty;
    }

    /// <summary>
    /// Checks if a containerized process is running via Docker API.
    /// </summary>
    /// <param name="process">The process to check.</param>
    /// <returns>True if the container is running; false otherwise.</returns>
    public async Task<bool> CheckProcess(Process process)
    {
        ValidateProcessProperties(process);

        try
        {
            DockerClient client = new DockerClientConfiguration(
                    new Uri(process.HostAddr!))
                .CreateClient();

            var containerList = await client.Containers.ListContainersAsync(
                new ContainersListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        { "id", new Dictionary<string, bool> { { process.DockerContainerId ?? string.Empty, true } } }
                    }
                });

            return containerList.Any(c => c.State == "running");
        }
        catch
        {
            logger.LogWarning($"Error when checking process {process.Name} id {process.Id} under session {process.SessionId}.");
        }

        return false;
    }

    /// <summary>
    /// Stops a containerized process via Docker API.
    /// </summary>
    /// <param name="process">The process to stop.</param>
    public async Task KillProcess(Process process)
    {
        ValidateProcessProperties(process);

        try
        {
            DockerClient client = new DockerClientConfiguration(
                    new Uri(process.HostAddr!))
                .CreateClient();

            await client.Containers.KillContainerAsync(
                process.DockerContainerId,
                new ContainerKillParameters(),
                CancellationToken.None);
        }
        catch
        {
            logger.LogWarning($"Error when checking process {process.Name} id {process.Id} under session {process.SessionId}.");
        }
    }

    /// <summary>
    /// Validates required properties of the process for Docker operations.
    /// </summary>
    /// <param name="process">The process to validate.</param>
    /// <exception cref="ArgumentException">Thrown when a required property is missing or invalid.</exception>
    private void ValidateProcessProperties(Process process)
    {
        if (string.IsNullOrWhiteSpace(process.HostAddr))
        {
            throw new ArgumentException("Docker process requires a valid 'HostAddr'.");
        }

        if (string.IsNullOrWhiteSpace(process.DockerContainerId))
        {
            throw new ArgumentException("Docker process requires a valid 'DockerContainerId'.");
        }
    }
}
