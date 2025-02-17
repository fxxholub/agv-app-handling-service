using System.ComponentModel.DataAnnotations;

namespace Handling_Service.WebAPI.Models.Session;

public class SessionRequestModel
{
  [Required]
  public string? HandlingMode { get; set; }
  
}