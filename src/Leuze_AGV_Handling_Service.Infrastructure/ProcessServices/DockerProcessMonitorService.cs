using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure.Exceptions;
using Renci.SshNet;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

/// <summary>
/// Handles process starting, checking and killing via Docker API.
/// </summary>
public class DockerProcessMonitorService: IProcessMonitorService
{
    /// <summary>
    /// TODO:
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public async Task<string> StartProcess(Process process)
    {
        return await Task.FromResult("123");
    }

    /// <summary>
    /// TODO:
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    /// <exception cref="EnvironmentVariableNullException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<bool> CheckProcess(Process process)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// Makes SSH connection and kills process as entire process tree.
    /// </summary>
    /// <param name="process"></param>
    public async Task KillProcess(Process process)
    {
        await Task.Delay(100);
    }

    // /// <summary>
    // /// Validates process properties for SSH connection usage.
    // /// </summary>
    // /// <param name="process"></param>
    // /// <returns></returns>
    // /// <exception cref="ArgumentException"></exception>
    // private (string, string, string) ValidateProcessProperties(Process process)
    // {
    //     if (String.IsNullOrEmpty(process.HostAddr))
    //     {
    //         throw new ArgumentException($"Process handled with SSH requires HostAddr.");
    //     }
    //     if (String.IsNullOrEmpty(process.UserName))
    //     {
    //         throw new ArgumentException($"Process handled with SSH requires UserName.");
    //     }
    //     if (String.IsNullOrEmpty(process.PrivateKeyPath))
    //     {
    //         throw new ArgumentException($"Process handled with SSH requires PrivateKeyPath.");
    //     }
    //     return (process.HostAddr, process.UserName, process.PrivateKeyPath);
    // }
}
