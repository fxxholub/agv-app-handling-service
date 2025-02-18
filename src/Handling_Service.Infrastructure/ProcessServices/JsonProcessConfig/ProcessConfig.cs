namespace Handling_Service.Infrastructure.ProcessServices.JsonProcessConfig;

public class ProcessConfig
{
    public List<ProcessC> Common { get; set; } = new();
    public List<ProcessC> Manual { get; set; } = new();
    public List<ProcessC> Autonomous { get; set; } = new();
    
    public List<ProcessC> Automatic { get; set; } = new();
    
    public List<ProcessC> Linefollow { get; set; } = new();

    public void Validate()
    {
        if (Common == null) throw new InvalidOperationException("Common processes cannot be null.");
        if (Manual == null) throw new InvalidOperationException("Manual processes cannot be null.");
        if (Autonomous == null) throw new InvalidOperationException("Autonomous processes cannot be null.");
        if (Automatic == null) throw new InvalidOperationException("Manual processes cannot be null.");
        if (Linefollow == null) throw new InvalidOperationException("Autonomous processes cannot be null.");

        foreach (var process in Common)
        {
            process.Validate();
        }
        
        foreach (var process in Manual)
        {
            process.Validate();
        }

        foreach (var process in Autonomous)
        {
            process.Validate();
        }
        
        foreach (var process in Automatic)
        {
            process.Validate();
        }
        
        foreach (var process in Linefollow)
        {
            process.Validate();
        }
    }
}