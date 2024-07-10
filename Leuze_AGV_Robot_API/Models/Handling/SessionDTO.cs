using MongoDB.Bson;
using Mono.TextTemplating;

namespace Leuze_AGV_Robot_API.Models.Handling
{
    public class SessionDTO
    {
        public string? Id { get; set; }
        public string? State { get; set; }
        public string? Status { get; set; }
        public string? StatusMessage { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public IList<ObjectId>? Processes { get; set; }

        public bool MappingEnabled { get; set; }
        public ObjectId? MapInputId { get; set; }
        public string? MapOutputName { get; set; }
        public ObjectId? MapOutputId { get; set; }
    }
}
