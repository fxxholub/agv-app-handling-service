using System.ComponentModel.DataAnnotations;

namespace Handling_Service.WebAPI.Models.Session;

public class ActionRequestModel
{
  [Required]
  public string? Command { get; set; }
}