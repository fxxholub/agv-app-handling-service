using Leuze_AGV_Robot_API.Interfaces;
using MongoDB.Bson;
using Mono.TextTemplating;
using Realms;

namespace Leuze_AGV_Robot_API.Models.Handling
{
    
    public partial class SessionModel : IRealmObject, IDTO<SessionDTO>
    {
        private string _State { get; set; } = SessionState.IDLE.ToString();
        private string _Status { get; set; } = SessionStatus.NONE.ToString();

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("state")]
        public SessionState State
        {
            get => Enum.Parse<SessionState>(_State);
            set => _State = value.ToString();
        }

        [MapTo("status")]
        public SessionStatus Status
        {
            get => Enum.Parse<SessionStatus>(_Status);
            set => _Status = value.ToString();
        }

        [MapTo("statusMessage")]
        public string StatusMessage { get; set; } = "";

        [MapTo("createdDate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [MapTo("modifiedDate")]
        public DateTimeOffset ModifiedDate { get; set; } = DateTimeOffset.UtcNow;

        [MapTo("processes")]
        public IList<ObjectId> Processes { get; } = null!;

        [MapTo("mappingEnabled")]
        public bool MappingEnabled { get; set; }

        [MapTo("mapInputId")]
        public ObjectId? MapInputId { get; set; }

        [MapTo("mapOutputName")]
        public string? MapOutputName {  get; set; }

        [MapTo("mapOutputId")]
        public ObjectId? MapOutputId { get; set; }

        public SessionDTO ToDTO()
        {
            return new SessionDTO
            {
                Id = Id.ToString(),
                State = State.ToString(),
                Status = Status.ToString(),
                CreatedDate = CreatedDate,
                ModifiedDate = ModifiedDate,
                Processes = Processes,
                MappingEnabled = MappingEnabled,
                MapInputId = MapInputId,
                MapOutputName = MapOutputName,
                MapOutputId = MapOutputId,
            };
        }
    }
}
