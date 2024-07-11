using MongoDB.Bson;
using Mono.TextTemplating;

namespace Leuze_AGV_Robot_API.Models.Handling
{
    public class SessionGetDTO
    {
        public string? Id { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public IList<string> Processes { get; set; }

        public bool MappingEnabled { get; set; }
        public string? MapInputId { get; set; }
        public string? MapOutputName { get; set; }
        public string? MapOutputId { get; set; }
    }
}
