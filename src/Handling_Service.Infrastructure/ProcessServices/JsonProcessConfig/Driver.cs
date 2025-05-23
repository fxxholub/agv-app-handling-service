using System.Text.Json.Serialization;

namespace Handling_Service.Infrastructure.ProcessServices.JsonProcessConfig;

public class Driver
{
    public string Type { get; set; } = string.Empty; // "ssh" or "docker"
    public string Address { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Auth? Auth { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Commands { get; set; } // Only for SSH
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContainerId { get; set; } // Only for Docker

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Type))
            throw new InvalidOperationException("Driver 'Type' cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(Address))
            throw new InvalidOperationException("Driver 'Address' cannot be null or empty.");
        
        if (Auth != null)
        {
            switch (Type.ToLower())
            {
                case "docker":
                    ValidateDockerAuth();
                    break;
                case "ssh":
                    ValidateSshAuth();
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported Driver 'Type': {Type}");
            }
        }
        else
        {
            // If Auth is required for both drivers, you can uncomment the following line:
            // throw new InvalidOperationException("Auth cannot be null.");
        }

        switch (Type.ToLower())
        {
            case "docker":
                ValidateDockerProperties();
                break;
            case "ssh":
                ValidateSshProperties();
                break;
            default:
                throw new InvalidOperationException($"Unsupported Driver 'Type': {Type}");
        }
    }

    private void ValidateDockerAuth()
    {
        if (string.IsNullOrWhiteSpace(Auth?.Username) || string.IsNullOrWhiteSpace(Auth?.Password))
        {
            throw new InvalidOperationException("Docker Driver requires 'Username' and 'Password' in 'Auth' when provided.");
        }

        if (!string.IsNullOrEmpty(Auth?.PrivateKeyFile))
        {
            throw new InvalidOperationException("Docker Driver does not support 'PrivateKeyFile' in 'Auth'.");
        }
    }

    private void ValidateSshAuth()
    {
        if (string.IsNullOrWhiteSpace(Auth?.Username))
        {
            throw new InvalidOperationException("SSH Driver requires 'Username' in 'Auth'.");
        }

        if (string.IsNullOrWhiteSpace(Auth?.PrivateKeyFile))
        {
            throw new InvalidOperationException("SSH Driver requires 'PrivateKeyFile' in 'Auth'.");
        }

        if (!string.IsNullOrEmpty(Auth?.Password))
        {
            throw new InvalidOperationException("SSH Driver does not support 'Password' in 'Auth'.");
        }
    }
    
    private void ValidateDockerProperties()
    {
        if (string.IsNullOrWhiteSpace(ContainerId))
            throw new InvalidOperationException("Docker Driver requires 'ContainerId' to be specified.");

        if (Commands != null && Commands.Count > 0)
            throw new InvalidOperationException("Docker Driver does not support 'Commands'.");
    }

    private void ValidateSshProperties()
    {
        if (Commands == null)
            throw new InvalidOperationException("SSH Driver requires 'Commands' to be provided (can be empty).");

        if (!string.IsNullOrEmpty(ContainerId))
            throw new InvalidOperationException("SSH Driver does not support 'ContainerId'.");
    }
}