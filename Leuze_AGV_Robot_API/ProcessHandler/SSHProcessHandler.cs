using Asp.Versioning;
using Renci.SshNet;

namespace Leuze_AGV_Robot_API.ProcessHandler
{
    public class SSHProcessHandler(string host, string username, string privateKeyPath)
    {
        public readonly string host = host;
        public readonly string username = username;
        private readonly string privateKeyPath = privateKeyPath;

        public static List<string> LoadScript(string scriptFilePath)
        {
            var lines = new List<string>();

            // Read all lines from the script file
            foreach (var line in File.ReadLines(scriptFilePath))
            {
                // Trim the line and add it if it's not empty
                var trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    lines.Add(trimmedLine);
                }
            }

            return lines;
        }

        public string StartProcess(string scriptFilePath)
        {
            var commands = LoadScript(scriptFilePath);

            string combinedCommands = string.Join(" && ", commands); // pipeline commands, && - run next only if current succeeded

            // Add the command to run in the background and get the PID
            string detachedCommand = $"nohup bash -c \"{combinedCommands}\" > /dev/null 2>&1 & echo $!";

            using (var client = new SshClient(host, username, new PrivateKeyFile(privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand(detachedCommand);
                var pid = cmd.Execute().Trim();
                client.Disconnect();

                return pid;
            }
        }
        public bool CheckProcess(string pid)
        {
            using (var client = new SshClient(host, username, new PrivateKeyFile(privateKeyPath)))
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
                    throw new Exception($"Process check on host {host} under user {username} with pid {pid} has failed.");
                }
            }
        }

        public void KillProcess(string pid)
        {
            using (var client = new SshClient(host, username, new PrivateKeyFile(privateKeyPath)))
            {
                client.Connect();
                var cmd = client.CreateCommand($"kill {pid}");
                cmd.Execute();
                client.Disconnect();
            }
        }
    }
}
