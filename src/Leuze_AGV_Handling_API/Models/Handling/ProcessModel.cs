using Realms;
using MongoDB.Bson;
using SignalR_API.StateMachine;

namespace SignalR_API.Models.Handling
{
    public partial class ProcessModel : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("name")]
        public string Name { get; set; }

        [MapTo("pid")]
        public string Pid { get; set; }

        [MapTo("active")]
        public bool Active { get; set; }

        [MapTo("host")]
        public string Host { get; set; }

        [MapTo("user")]
        public string User { get; set; }

        [MapTo("createdDate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    }
}
