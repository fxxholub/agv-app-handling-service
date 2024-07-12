using Realms;
using MongoDB.Bson;
using Leuze_AGV_Robot_API.StateMachine;

namespace Leuze_AGV_Robot_API.Models.Handling
{
    public partial class ActionModel : IRealmObject
    {
        private string _Command { get; set; }/* = SessionActionCommand.STOP.ToString();*/

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("command")]
        public ActionCommand Command
        {
            get => Enum.Parse<ActionCommand>(_Command);
            set => _Command = value.ToString();
        }

        [MapTo("targetPosition")]
        public string? TargetPosition { get; set; }

        [MapTo("createdDate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    }
}
