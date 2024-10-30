namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models;

public class CreateSessionRequestModel
{
    public bool MappingEnabled { get; set; }
  
    public string? InputMapRef { get; set; }
  
    public string? OutputMapRef { get; set; }
  
    public string? OutputMapName { get; set; }
}