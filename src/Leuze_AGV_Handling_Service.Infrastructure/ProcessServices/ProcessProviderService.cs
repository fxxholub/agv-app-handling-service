using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.Infrastructure.ProcessServices.JsonProcessConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Process = Leuze_AGV_Handling_Service.Core.Session.SessionAggregate.Process;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

public class ProcessProviderService : IProcessProviderService
{
    private readonly Dictionary<HandlingMode, List<Process>> _loadedProcesses = new();

    private readonly string? _processScriptsPath; 

    public ProcessProviderService(IConfiguration configuration, ILogger<ProcessProviderService> logger)
    {
        // Prepopulate _loadedProcesses with all keys from HandlingMode enum
        foreach (var mode in Enum.GetValues<HandlingMode>())
        {
            _loadedProcesses[mode] = new List<Process>();
        }

        // Retrieve config file path from appsettings configuration
        _processScriptsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            configuration["ProcessScripts:Path"] ??
            throw new ArgumentException("ProcessProviderService Appsettings ProcessScripts:Path not found.")
        );

        logger.LogInformation($"ProcessProviderService will load process config from {_processScriptsPath}");

        // Retrieve the JSON data
        string jsonStr = File.ReadAllText(Path.Combine(_processScriptsPath, "config.json"));
        if (jsonStr is null) throw new ArgumentException("ConfigFileProcessProvider config json string is null.");
        ProcessConfig config = ProcessConfigParser.Parse(jsonStr);

        // Load Common processes
        foreach (var confProcess in config.Common)
        {
            AddProcessToAllModes(confProcess);
        }

        // Load Manual processes
        foreach (var confProcess in config.Manual)
        {
            AddProcessToMode(confProcess, HandlingMode.Manual);
        }

        // Load Autonomous processes
        foreach (var confProcess in config.Autonomous)
        {
            AddProcessToMode(confProcess, HandlingMode.Autonomous);
        }
    }

    public IEnumerable<Process> GetProcesses(HandlingMode handlingMode)
    {
        if (_loadedProcesses.TryGetValue(handlingMode, out var processList))
        {
            return processList;
        }

        return [];
    }

    private void AddProcessToAllModes(ProcessC confProcess)
    {
        var process = CreateProcess(confProcess);
        foreach (var mode in _loadedProcesses.Keys)
        {
            _loadedProcesses[mode].Add(process);
        }
    }

    private void AddProcessToMode(ProcessC confProcess, HandlingMode mode)
    {
        var process = CreateProcess(confProcess);
        _loadedProcesses[mode].Add(process);
    }

    private Process CreateProcess(ProcessC confProcess)
    {
        string? username = null;
        string? password = null;
        string? privateKeyFile = null;

        if (confProcess.Driver.Auth != null)
        {
            username = confProcess.Driver.Auth.Username;
            password = confProcess.Driver.Auth.Password;
            privateKeyFile = _processScriptsPath != null && confProcess.Driver.Auth?.PrivateKeyFile != null
                ? Path.Combine(_processScriptsPath, confProcess.Driver.Auth.PrivateKeyFile)
                : null;
        }

        if (!Enum.TryParse<DriverType>(confProcess.Driver.Type, true, out var driverType))
        {
            throw new ArgumentException($"Invalid driver type: {confProcess.Driver.Type}");
        }

        var process =  new Process(
            confProcess.Name,
            driverType,
            confProcess.HostName,
            confProcess.Driver.Address,
            username,
            password,
            null,
            privateKeyFile,
            confProcess.Driver.ContainerId
        );
        
        // add sh commands (if any)
        if (confProcess.Driver.Commands is not null)
            process.AddCommands(confProcess.Driver.Commands.Select(line => new ProcessCommand(line)));

        return process;
    }
}
