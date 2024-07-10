using Leuze_AGV_Robot_API.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Leuze_AGV_Robot_API.Models
{
    public class RealmItemDTO
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string Assignee { get; set; }
    }
    public partial class RealmItem : IRealmObject, IDTO<RealmItemDTO>
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("state")]
        public string? State { get; set; }

        [MapTo("assignee")]
        public string Assignee { get; set; }


        [MapTo("name")]
        public string? Name { get; set; }

        public RealmItemDTO ToDTO()
        {
            return new RealmItemDTO
            {
                Id = Id.ToString(),
                Name = Name,
                Assignee = Assignee
            };
        }
    }

}
