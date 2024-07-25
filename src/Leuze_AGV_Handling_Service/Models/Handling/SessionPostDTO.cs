using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Handling
{
    public class SessionPostDTO
    {
        [Required]
        public bool MappingEnabled { get; set; }
        public string? MapInputId { get; set; }
        public string? MapOutputName { get; set; }
        public string? MapOutputId { get; set; }
    }
}
