namespace Leuze_AGV_Handling_Service.Infrastructure.ProcessServices.JsonProcessConfig;

public class ProcessC
{
    public string Name { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public Driver Driver { get; set; } = new();

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidOperationException("Process 'Name' cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(HostName))
            throw new InvalidOperationException("Process 'HostName' cannot be null or empty.");

        if (Driver == null)
            throw new InvalidOperationException("Process 'Driver' cannot be null.");

        Driver.Validate();
    }
}