using MongoDB.Bson;
using Mono.TextTemplating;

namespace Leuze_AGV_Robot_API.Models.Handling
{
    public class SessionPostDTO
    {
        public bool MappingEnabled { get; set; }
        public string? MapInputId { get; set; }
        public string? MapOutputName { get; set; }
        public string? MapOutputId { get; set; }
    }
}
