using System.Text.Json.Serialization;

namespace Handling_Service.Infrastructure.ProcessServices.JsonProcessConfig;

public class Auth
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PrivateKeyFile { get; set; } // Only for SSH
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Username { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Password { get; set; }
}