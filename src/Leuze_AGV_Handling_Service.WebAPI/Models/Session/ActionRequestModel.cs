using System.ComponentModel.DataAnnotations;

namespace Leuze_AGV_Handling_Service.WebAPI.Models.Session;

public class ActionRequestModel
{
  [Required]
  public string? Command { get; set; }
}