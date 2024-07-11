using MongoDB.Bson;
using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;

namespace Leuze_AGV_Robot_API.Models.Handling
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
