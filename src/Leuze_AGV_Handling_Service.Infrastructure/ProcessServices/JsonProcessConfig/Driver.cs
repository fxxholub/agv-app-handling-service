using System.Text.Json.Serialization;

namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices.JsonProcessConfig;

public class Driver
{
    public string Type { get; set; } = string.Empty; // "ssh" or "docker"
    public string Address { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Auth? Auth { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Commands { get; set; } // Only for SSH
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Image { get; set; } // Only for Docker
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Tag { get; set; } // Only for Docker
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Platform { get; set; } // Only for Docker

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
    }

    private void ValidateDockerAuth()
    {
        if (string.IsNullOrWhiteSpace(Auth?.Username) || string.IsNullOrWhiteSpace(Auth?.Password))
            throw new InvalidOperationException("Docker Driver requires 'Username' and 'Password' in 'Auth' when provided.");
        if (!string.IsNullOrEmpty(Auth?.PrivateKeyFile))
            throw new InvalidOperationException("Docker Driver does not support 'PrivateKeyFile' in 'Auth'.");
    }

    private void ValidateSshAuth()
    {
        bool hasCredentials = !string.IsNullOrWhiteSpace(Auth?.Username) && !string.IsNullOrWhiteSpace(Auth?.Password);
        bool hasPrivateKey = !string.IsNullOrWhiteSpace(Auth?.PrivateKeyFile);

        if (!hasCredentials && !hasPrivateKey)
            throw new InvalidOperationException("SSH Driver requires either 'Username' and 'Password' or 'PrivateKeyFile' in 'Auth'.");
    }
}