using System.ComponentModel.DataAnnotations;

namespace Leuze_AGV_Handling_Service.WebAPI.Models.Session;

public class SessionRequestModel
{
  [Required]
  public bool MappingEnabled { get; set; }
  
  public string? InputMapRef { get; set; }
  
  public string? OutputMapRef { get; set; }
  
  public string? OutputMapName { get; set; }
  
}