using Leuze_AGV_Handling_Service.Core.Exceptions;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Renci.SshNet;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

public class SshProcessHandlerService(string privateKeyPath): IProcessHandlerService
{
    private readonly string _privateKeyPath = privateKeyPath;
    public async Task<string> StartProcess(Process process)
    {
        return await Task.Run(() =>
        {
          // pipeline commands, && - run next only if current succeeded
            string combinedCommands = string.Join(" && ", process.Commands);

            // Add the command to run in the background and get the PID
            string detachedCommand = $"nohup bash -c \"{combinedCommands}\" > /dev/null 2>&1 & echo $!";

            using (var client = new SshClient(process.HostAddr, process.UserName, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand(detachedCommand);
                var pid = cmd.Execute().Trim();
                client.Disconnect();

                return pid;
            }
        });
    }

    public async Task<bool> CheckProcess(Process process)
    {
        if (String.IsNullOrEmpty(process.Pid))
        {
            throw new EnvironmentVariableNullException($"Cannot check process {process.Name} with null or empty Pid.");
        }
        
        return await Task.Run(() =>
        {
            using (var client = new SshClient(process.HostAddr, process.UserName, new PrivateKeyFile(_privateKeyPath)))
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

    public async Task KillProcess(Process process)
    {
        await Task.Run(() =>
        {
            using (var client = new SshClient(process.HostAddr, process.UserName, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand($"kill {process.Pid}");
                cmd.Execute();
                client.Disconnect();
            }
        });
    }
}
