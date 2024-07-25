using System.ComponentModel.DataAnnotations;

namespace SignalR_API.Models.Handling
{
    public class ActionPostDTO
    {
        [Required]
        public string Command { get; set; }
        public string? TargetPosition { get; set; }
    }
}
