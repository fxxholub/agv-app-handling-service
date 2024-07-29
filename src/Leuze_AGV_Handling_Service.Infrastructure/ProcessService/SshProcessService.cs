using Leuze_AGV_Handling_Service.Core.Interfaces;
using Renci.SshNet;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessService;

public class SshProcessService(string address, string username, string privateKeyPath): IProcessService
{
    private readonly string _address = address;
    private readonly string _username = username;
    private readonly string _privateKeyPath = privateKeyPath;

    public async Task<string> StartProcess(IEnumerable<string> commands)
    {
        return await Task.Run(() =>
        {
          // pipeline commands, && - run next only if current succeeded
            string combinedCommands = string.Join(" && ", commands);

            // Add the command to run in the background and get the PID
            string detachedCommand = $"nohup bash -c \"{combinedCommands}\" > /dev/null 2>&1 & echo $!";

            using (var client = new SshClient(_address, _username, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand(detachedCommand);
                var pid = cmd.Execute().Trim();
                client.Disconnect();

                return pid;
            }
        });
    }

    public async Task<bool> CheckProcess(string pid)
    {
        return await Task.Run(() =>
        {
            using (var client = new SshClient(_address, _username, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand($"ps -p {pid} > /dev/null && echo \"true\" || echo \"false\"");
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
                    throw new Exception($"Process check on host {_address} under user {_username} with pid {pid} has failed.");
                }
            }
        });
    }

    public async Task KillProcess(string pid)
    {
        await Task.Run(() =>
        {
            using (var client = new SshClient(_address, _username, new PrivateKeyFile(_privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand($"kill {pid}");
                cmd.Execute();
                client.Disconnect();
            }
        });
    }
}
