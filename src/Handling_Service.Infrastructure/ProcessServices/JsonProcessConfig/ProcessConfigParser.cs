using System.Text.Json;

namespace Handling_Service.Infrastructure.ProcessServices.JsonProcessConfig;

public class ProcessConfigParser
{
    public static ProcessConfig Parse(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        var config = JsonSerializer.Deserialize<ProcessConfig>(json, options);

        if (config == null)
        {
            throw new InvalidOperationException("Failed to parse ProcessConfig from JSON.");
        }

        config.Validate();
        return config;
    }
}