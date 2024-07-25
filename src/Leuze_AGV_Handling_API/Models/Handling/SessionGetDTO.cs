using System.ComponentModel.DataAnnotations;

namespace SignalR_API.Models.Handling
{
    public class SessionGetDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Mode { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 0)]
        public string StateMessage { get; set; }
        [Required]
        public IList<ActionGetDTO> Actions { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        [Required]
        public DateTimeOffset ModifiedDate { get; set; }
        [Required]
        public IList<ProcessGetDTO> Processes { get; set; }
        [Required]
        public bool MappingEnabled { get; set; }
        public string? MapInputId { get; set; }
        public string? MapOutputName { get; set; }
        public string? MapOutputId { get; set; }
    }
}
