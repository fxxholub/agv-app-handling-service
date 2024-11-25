using System.ComponentModel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

/// <summary>
/// Fetches processes from ProcessScripts directory. Should be registered as Singleton.
/// </summary>
public class FileProcessProviderService : IProcessProviderService
{
    private readonly Dictionary<HandlingMode, List<Process>> _loadedProcesses = new();

    /// <summary>
    /// Fetches process scripts and makes process instances out of them.
    /// Follows the rule set specified in /ProcessScripts readme.md.
    /// </summary>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public FileProcessProviderService(IConfiguration configuration, ILogger<FileProcessProviderService> logger)
    {
        var processScriptsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            configuration["ProcessScripts:Path"] ?? throw new ArgumentException("Appsettings ProcessScripts:Path not found.")
            );
        
        logger.LogInformation($"FileProcessProvider will load scripts from {processScriptsPath}");

        // get ProcessScripts directory
        if (!Directory.Exists(processScriptsPath))
        {
            throw new DirectoryNotFoundException($"ProcessScripts directory '{processScriptsPath}' was not found.");
        }
        if (Path.GetFileNameWithoutExtension(processScriptsPath) != "ProcessScripts")
        {
            throw new DirectoryNotFoundException($"ProcessScripts directory must be named ProcessScripts directory, is '{Path.GetFileNameWithoutExtension(processScriptsPath)}'.");
        }
        
        // get folders (hosts / host connections)
        var hostFolders = Directory.GetDirectories(processScriptsPath);

        if (hostFolders.Length == 0)
        {
            logger.LogWarning("FileProcessProvider no host folder loaded.");
        }
        
        // prepare handling modes for later check
        var handlingModes = new List<HandlingMode>(Enum.GetValues<HandlingMode>());
        var handlingNames = handlingModes.Select(mode => mode.ToString()).ToList();
        
        // host folders
        foreach (var hostFolder in hostFolders)
        {
            var hostFolderName = Path.GetFileNameWithoutExtension(hostFolder);
            if (String.IsNullOrEmpty(hostFolderName))
            {
                throw new InvalidEnumArgumentException($"ProcessScripts Folder {hostFolderName} name is null or empty.");
            }
            
            // prepare config and private key
            var hostConfig = ParseConfig(Path.Join(hostFolder, "config.json"));
            var privateKeyPath = Path.Join(hostFolder, ".private_key");
            if (!Path.Exists(privateKeyPath))
            {
                throw new FileNotFoundException($"ProcessScripts file {privateKeyPath} not found.");
            }
            
            // get hosts subfolders - handling modes
            var handlingFolders = Directory.GetDirectories(hostFolder);
            
            if (handlingFolders.Length == 0)
            {
                logger.LogWarning($"FileProcessProvider no handling mode folder loaded for host {Path.GetFileName(hostFolder)}.");
            }

            // hosts handling folders
            foreach (var handlingFolder in handlingFolders)
            {
                var handlingFolderName = Path.GetFileName(handlingFolder);
                
                // check if handling folder has correct name
                if (!(handlingFolderName == "Common" || (handlingNames.Any(name => name.Equals(handlingFolderName)))))
                {
                    throw new InvalidEnumArgumentException($"ProcessScripts host {hostFolderName} subfolder {handlingFolderName} is neither Common nor valid HandlingMode.");
                }
                
                // get .sh scripts
                var scriptFiles = Directory.GetFiles(handlingFolder, "*.sh");
                
                if (handlingFolders.Length == 0)
                {
                    logger.LogWarning($"FileProcessProvider no scripts loaded for host {Path.GetFileName(hostFolder)} for mode {handlingFolderName}.");
                }
                
                // script files
                foreach (var scriptFile in scriptFiles)
                {
                    var scriptName = Path.GetFileNameWithoutExtension(scriptFile);
                    
                    // get lines as commands
                    var commands = File.ReadAllLines(scriptFile)
                        .Where(line => !(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)))
                        .Select(line => line.Trim())
                        .Select(line => new ProcessCommand(line))
                        .ToList();

                    // create Process object and commands
                    Process process = new Process(
                        scriptName,
                        hostConfig.HostName,
                        hostConfig.HostAddr,
                        hostConfig.HostUser,
                        null,
                        privateKeyPath
                        );
                    process.AddCommands(commands);

                    // Store loaded Processes to memory for later use
                    if (handlingFolderName == "Common")
                    {
                        foreach (var handlingMode in handlingNames)
                        {
                            AddProcess(Enum.Parse<HandlingMode>(handlingMode), process);
                        }
                    }
                    else
                    {
                        AddProcess(Enum.Parse<HandlingMode>(handlingFolderName), process);
                    }
                }
            }
        }
    }
    
    private void AddProcess(HandlingMode handlingMode, Process process)
    {
        if (_loadedProcesses.TryGetValue(handlingMode, out var processList))
        {
            processList.Add(process);
        }
        else
        {
            _loadedProcesses[handlingMode] = new List<Process> { process };
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

    private static ProcessConfig ParseConfig(string configPath)
    {
        var jsonString = File.ReadAllText(configPath);
        var config = JsonConvert.DeserializeObject<ProcessConfig>(jsonString);
        if (config == null)
        {
            throw new InvalidOperationException("ProcessScripts to deserialize the configuration file.");
        }
        return config;
    }
    
    private class ProcessConfig
    {
        public string? HostName { get; set; }
        public string? HostAddr { get; set; }
        public string? HostUser { get; set; }
    }
}
