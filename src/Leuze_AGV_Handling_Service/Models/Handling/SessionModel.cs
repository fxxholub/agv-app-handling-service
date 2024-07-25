using MongoDB.Bson;
using Realms;
using WebAPI.StateMachine;

namespace WebAPI.Models.Handling
{

    public partial class SessionModel : IRealmObject
    {
        private string? _Mode { get; set; }
        private string _State { get; set; } = SessionState.IDLING.ToString();

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("mode")]
        public string Mode { get; set; }

        [MapTo("state")]
        public SessionState State
        {
            get => Enum.Parse<SessionState>(_State);
            set => _State = value.ToString();
        }

        [MapTo("stateMessage")]
        public string StateMessage { get; set; } = "";

        [MapTo("actions")]
        public IList<ActionModel> Actions { get; }

        [MapTo("createdDate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [MapTo("modifiedDate")]
        public DateTimeOffset ModifiedDate { get; set; } = DateTimeOffset.UtcNow;

        [MapTo("processes")]
        public IList<ProcessModel> Processes { get; } = null!;

        [MapTo("mappingEnabled")]
        public bool MappingEnabled { get; set; }

        [MapTo("mapInputId")]
        public ObjectId? MapInputId { get; set; }

        [MapTo("mapOutputName")]
        public string? MapOutputName {  get; set; }

        [MapTo("mapOutputId")]
        public ObjectId? MapOutputId { get; set; }
    }
}
