using System.ComponentModel;
using Leuze_AGV_Handling_Service.Core.Session.Interfaces;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Newtonsoft.Json;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

/// <summary>
/// Fetches processes from ProcessScripts directory. Should be registered as Singleton.
/// </summary>
public class FileProcessProviderService: IProcessProviderService
{
    private readonly string _processScriptsPath;
    private readonly Dictionary<HandlingMode, List<Process>> _loadedProcesses = new();

    /// <summary>
    /// Fetches process scripts and makes process instances out of them.
    /// Follows the rule set specified in /ProcessScripts readme.md.
    /// </summary>
    /// <param name="processScriptsPath"></param>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public FileProcessProviderService(string processScriptsPath)
    {
        _processScriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, processScriptsPath);
        
        // get ProcessScripts directory
        if (!Directory.Exists(_processScriptsPath))
        {
            throw new DirectoryNotFoundException($"ProcessScripts directory '{_processScriptsPath}' was not found.");
        }
        if (Path.GetFileNameWithoutExtension(_processScriptsPath) != "ProcessScripts")
        {
            throw new DirectoryNotFoundException($"ProcessScripts directory must be named ProcessScripts directory, is '{Path.GetFileNameWithoutExtension(_processScriptsPath)}'.");
        }
        
        // get folders (hosts / host connections)
        var hostFolders = Directory.GetDirectories(_processScriptsPath);
        
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
                
                // script files
                foreach (var scriptFile in scriptFiles)
                {
                    var scriptName = Path.GetFileNameWithoutExtension(scriptFile);
                    
                    // get lines
                    var lines = File.ReadAllLines(scriptFile)
                        .Where(line => !(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)))
                        .Select(line => line.Trim())
                        .ToList();

                    Process process = new Process(
                        scriptName,
                        hostConfig.HostName,
                        hostConfig.HostAddr,
                        hostConfig.HostUser,
                        null,
                        privateKeyPath
                        );
                    process.AddCommands(new List<string>(lines));

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
        return _loadedProcesses[handlingMode];
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
