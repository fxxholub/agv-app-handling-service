using SignalR_API.Models.Handling;
using SignalR_API.ProcessHandler;
using SignalR_API.RealmDB;
using Realms;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SignalR_API.StateMachine
{
    public class AutonomousSessionStateMachine(string sessionId, Realm realm, string handlingMode): SessionStateMachineBase(sessionId, realm, handlingMode)
    {

        public override SessionState ChangeState(SessionState lastState, ActionCommand command)
        {
            SessionState nextState;

            try
            {
                nextState = (lastState, command) switch
                {
                    (SessionState.IDLING, ActionCommand.START) => Transition(SessionState.STARTED, OnStart),
                    (SessionState.STARTED, ActionCommand.END) => Transition(SessionState.ENDED, OnEnd),
                    _ => throw new NotSupportedException($"State '{lastState}' has no transition on '{command}' command")
                };
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred during state transition: {ex.Message}");
                nextState = SessionState.ENDED;
                throw;
            }

            return nextState;
        }

        private void OnStart()
        {
            // Initialise Process Handlers
            var djirpiHandler = new SSHProcessHandler("192.168.20.10", "jtvrz", "C:\\Users\\jholub\\.ssh\\djirpi");
            var leplinuxHandler = new SSHProcessHandler("192.168.20.20", "lepuser", "C:\\Users\\jholub\\.ssh\\leplinux");

            // Gather Scripts representing the processes
            string djirpiRelative = @"ProcessScripts\Autonomous\djirpi";
            string djirpiAbsolute = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, djirpiRelative));
            string[] djirpiScripts = Directory.GetFiles(djirpiAbsolute, "*", SearchOption.TopDirectoryOnly);

            string leplinuxRelative = @"ProcessScripts\Autonomous\leplinux";
            string leplinuxAbsolute = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, leplinuxRelative));
            string[] leplinuxScripts = Directory.GetFiles(leplinuxAbsolute, "*", SearchOption.TopDirectoryOnly);

            // Try to start the processes
            List<ProcessModel> djirpiProcesses = [];
            foreach (var scriptPath in djirpiScripts)
            { 
                string pid = djirpiHandler.StartProcess(scriptPath);

                ProcessModel process = new()
                {
                    Name = Path.GetFileName(scriptPath),
                    Pid = pid,
                    Active = true,
                    Host = "192.168.20.10",
                    User = "jtvrz"
                };
                djirpiProcesses.Add(process);
            }
            // wait some time before checking their status
            Thread.Sleep(1000);
            // Transactional manner - check if processes running
            bool djirpiProcessesRunning = true;
            foreach (var process in djirpiProcesses)
            {
                if(!djirpiHandler.CheckProcess(process.Pid))
                {
                    djirpiProcessesRunning = false;
                    Console.WriteLine($"process {process.Name} with pid {process.Pid} is not running.");
                }
            }
            // Transaction finalisation - register the processes in DB or kill them all
            if (djirpiProcessesRunning)
            {
                foreach (var process in djirpiProcesses)
                {
                    SessionDatabaseHandler.AddSessionProcess(realm, sessionId, process, handlingMode);
                    Console.WriteLine($"process {process.Name} with pid {process.Pid} started.");
                }
            }
            else
            {
                foreach (var process in djirpiProcesses)
                {
                    djirpiHandler.KillProcess(process.Pid);
                }
                throw new Exception($"Processes START transaction has failed");
            }

            // Try to start the processes
            List<ProcessModel> leplinuxProcesses = [];
            foreach (var scriptPath in leplinuxScripts)
            { 
                string pid = leplinuxHandler.StartProcess(scriptPath);

                ProcessModel process = new()
                {
                    Name = Path.GetFileName(scriptPath),
                    Pid = pid,
                    Active = true,
                    Host = "192.168.20.20",
                    User = "lepuser"
                };
                leplinuxProcesses.Add(process);
            }
            // wait some time before checking their status
            Thread.Sleep(1000);
            // Transactional manner - check if processes running
            bool leplinuxProcessesRunning = true;
            foreach (var process in leplinuxProcesses)
            {
                if(!leplinuxHandler.CheckProcess(process.Pid))
                {
                    leplinuxProcessesRunning = false;
                    Console.WriteLine($"process {process.Name} with pid {process.Pid} is not running.");
                }
            }
            // Transaction finalisation - register the processes in DB or kill them all
            if (leplinuxProcessesRunning)
            {
                foreach (var process in leplinuxProcesses)
                {
                    SessionDatabaseHandler.AddSessionProcess(realm, sessionId, process, handlingMode);
                    Console.WriteLine($"process {process.Name} with pid {process.Pid} started.");
                }
            }
            else
            {
                foreach (var process in leplinuxProcesses)
                {
                    leplinuxHandler.KillProcess(process.Pid);
                }
                throw new Exception($"Processes START transaction has failed");
            }
        }

        private void OnEnd()
        {
            var processes = SessionDatabaseHandler.GetSessionProcesses(realm, sessionId, handlingMode);

            var djirpiHandler = new SSHProcessHandler("192.168.20.10", "jholub", "C:\\Users\\jholub\\.ssh\\djirpi");
            var leplinuxHandler = new SSHProcessHandler("192.168.20.20", "lepuser", "C:\\Users\\jholub\\.ssh\\leplinux");

            foreach (var process in processes)
            {
                if (process.Host == djirpiHandler.host)
                {
                    if (!djirpiHandler.CheckProcess(process.Pid))
                    {
                        Console.WriteLine($"process {process.Name} with pid {process.Pid} were not running.");
                    } else
                    {
                        djirpiHandler.KillProcess(process.Pid);
                        SessionDatabaseHandler.SetSessionProcessActive(realm, sessionId, handlingMode, process.Id.ToString(), false);
                        Console.WriteLine($"process {process.Name} with pid {process.Pid} killed.");
                    }
                }

                if (process.Host == leplinuxHandler.host)
                {
                    if (!leplinuxHandler.CheckProcess(process.Pid))
                    {
                        Console.WriteLine($"process {process.Name} with pid {process.Pid} were not running.");
                    }
                    else
                    {
                        leplinuxHandler.KillProcess(process.Pid);
                        SessionDatabaseHandler.SetSessionProcessActive(realm, sessionId, handlingMode, process.Id.ToString(), false);
                        Console.WriteLine($"process {process.Name} with pid {process.Pid} killed.");
                    }
                }
            }
        }
    }
}
