using System.ComponentModel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices;

/// <summary>
/// Fetches processes from ProcessScripts directory. Should be registered as Singleton.
/// </summary>
public class FileProcessProviderService: IProcessProviderService
{
    private readonly string _processScriptsPath;
    private Dictionary<HandlingMode, List<Process>> _loadedProcesses = new();

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
        
        // get subdirectories (Common, Autonomous...)
        var subDirectories = Directory.GetDirectories(_processScriptsPath);
        
        foreach (var subDir in subDirectories)
        {
            var subDirName = Path.GetFileNameWithoutExtension(subDir);
            if (String.IsNullOrEmpty(subDirName))
            {
                throw new InvalidEnumArgumentException($"ProcessScripts Subdirectory {subDirName} name is null or empty.");
            }
            
            // get subfolders - patter hostName-hostAddr-userName
            var subFolders = Directory.GetDirectories(subDir);
            
            var handlingModes = new List<HandlingMode>(Enum.GetValues<HandlingMode>());
            var handlingNames = handlingModes.Select(mode => mode.ToString()).ToList();
            if (!(subDirName == "Common" || (handlingNames.Any(name => name.Equals(subDirName)))))
            {
                throw new InvalidEnumArgumentException($"ProcessScripts Subdirectory {subDirName} is neither Common nor valid HandlingMode.");
            }

            foreach (var subFol in subFolders)
            {
                // parse subFol by pattern
                var subFolName = Path.GetFileName(subFol);
                var parts = subFolName.Split('-');
                if (parts.Length != 3)
                {
                    throw new FormatException($"Subfolder name '{subFolName}' is not in the expected format 'hostName-hostAddr-userName'.");
                }
                string hostName = parts[0];
                string hostAddr = parts[1];
                string userName = parts[2];
                
                if (String.IsNullOrEmpty(hostName) || String.IsNullOrEmpty(hostAddr) || String.IsNullOrEmpty(userName))
                {
                    throw new FormatException($"Subfolder name '{subFolName}' is not in the expected format 'hostName-hostAddr-userName'.");
                }
                
                // get .sh scripts
                var scriptFiles = Directory.GetFiles(subFol, "*.sh");
                    
                foreach (var scriptFile in scriptFiles)
                {
                    // get lines
                    var lines = File.ReadAllLines(scriptFile);
                    // if (lines.IsNullOrEmpty())
                    // {
                    //     throw new InvalidDataException($"File {subFolName} doesnt contain any commands.");
                    // }

                    Process process = new Process(
                        Path.GetFileNameWithoutExtension(scriptFile),
                        hostName,
                        hostAddr,
                        userName,
                        null
                        );
                    process.AddCommands(new List<string>(lines));

                    if (subDirName == "Common")
                    {
                        foreach (var handlingMode in handlingNames)
                        {
                            AddProcess(Enum.Parse<HandlingMode>(handlingMode), process);
                        }
                    }
                    else
                    {
                        AddProcess(Enum.Parse<HandlingMode>(subDirName), process);
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
}
