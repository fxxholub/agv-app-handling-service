using Leuze_AGV_Handling_Service.Core.Exceptions;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Renci.SshNet;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

/// <summary>
/// Handles process starting, checking and killing via SSH connection.
/// The connection is made per action, immediately after it is closed.
/// </summary>
/// <param name="privateKeyPath"></param>
public class SshProcessHandlerService(string privateKeyPath): IProcessHandlerService
{
    private readonly string _privateKeyPath = privateKeyPath;
    
    /// <summary>
    /// Makes SSH connection and attempts to start given process.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public async Task<string> StartProcess(Process process)
    {
        return await Task.Run(() =>
        {
          // pipeline commands, && - run next only if current succeeded
            string combinedCommands = string.Join(" && ", process.Commands);

            // Add the command to run in the background and get the PID
            string detachedCommand = $"nohup bash -c \"{combinedCommands}\" > /dev/null 2>&1 & echo $!";

            var (addr, user) = ValidateProcessProperties(process);
            using (var client = new SshClient(addr, user, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand(detachedCommand);
                var pid = cmd.Execute().Trim();
                client.Disconnect();

                return pid;
            }
        });
    }

    /// <summary>
    /// Makes SSH connection and checks if process is alive.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    /// <exception cref="EnvironmentVariableNullException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<bool> CheckProcess(Process process)
    {
        if (String.IsNullOrEmpty(process.Pid))
        {
            throw new EnvironmentVariableNullException($"Cannot check process {process.Name} with null or empty Pid.");
        }
        
        return await Task.Run(() =>
        {
            var (addr, user) = ValidateProcessProperties(process);
            using (var client = new SshClient(addr, user, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand($"ps -p {process.Pid} > /dev/null && echo \"true\" || echo \"false\"");
                string result = cmd.Execute();
                client.Disconnect();

                if (result.Trim() == "true")
                {
                    return true;
                }
                else if (result.Trim() == "false")
                {
                    return false;
                }
                else
                {
                    throw new Exception($"Process check on host {process.HostAddr} under user {process.UserName} with pid {process.Pid} has failed.");
                }
            }
        });
    }

    /// <summary>
    /// Makes SSH connection and kills process.
    /// </summary>
    /// <param name="process"></param>
    public async Task KillProcess(Process process)
    {
        await Task.Run(() =>
        {
            var (addr, user) = ValidateProcessProperties(process);
            using (var client = new SshClient(addr, user, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand($"kill {process.Pid}");
                cmd.Execute();
                client.Disconnect();
            }
        });
    }

    /// <summary>
    /// Validates process properties for SSH connection usage.
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private (string, string) ValidateProcessProperties(Process process)
    {
        if (String.IsNullOrEmpty(process.HostAddr))
        {
            throw new ArgumentException($"Process handled with SSH requires HostAddr.");
        }
        if (String.IsNullOrEmpty(process.UserName))
        {
            throw new ArgumentException($"Process handled with SSH requires UserName.");
        }
        return (process.HostAddr, process.UserName);
    }
}
