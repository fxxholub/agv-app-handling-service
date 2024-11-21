using System.ComponentModel.DataAnnotations;

namespace Leuze_AGV_Handling_Service.WebAPI.Models.Session;

public class SessionRequestModel
{
  [Required]
  public string? HandlingMode { get; set; }
  
}