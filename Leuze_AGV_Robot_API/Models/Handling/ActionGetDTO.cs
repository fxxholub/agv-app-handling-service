using System.ComponentModel.DataAnnotations;

namespace Leuze_AGV_Robot_API.Models.Handling
{
    public class ActionGetDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Command { get; set; }
        public string? TargetPosition { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
