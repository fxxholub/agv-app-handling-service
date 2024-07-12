using Renci.SshNet;

namespace Leuze_AGV_Robot_API.ProcessHandler
{
    public static class SSHProcessHandler
    {
        private static string host = "192.168.20.10"; // Change to your host IP if necessary
        private static string username = "jholub"; // Host username
        private static string privateKey = "C:\\Users\\jholub\\.ssh\\djirpi";
        //"C:\Users\jholub\.ssh\djirpi"
        //"/root/.ssh/id_rsa"

        public static string StartCommand()
        {
            string command = "sleep 1000";
            string detachedCommand = $"nohup {command} > /dev/null 2>&1 & echo $!";

            using (var client = new SshClient(host, username, new PrivateKeyFile(privateKey)))
            {
                client.Connect();
                var cmd = client.CreateCommand(detachedCommand);
                var pid = cmd.Execute();
                client.Disconnect();

                Console.WriteLine(pid);

                return pid;
            }
        }

        public static void EndCommand(string pid)
        {
            using (var client = new SshClient(host, username, new PrivateKeyFile(privateKey)))
            {
                client.Connect();
                var cmd = client.CreateCommand($"kill {pid}");
                cmd.Execute();
                client.Disconnect();
            }
        }
    }
}
