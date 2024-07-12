using Asp.Versioning;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.RealmDB;
using Leuze_AGV_Robot_API.StateMachine;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Realms;

namespace Leuze_AGV_Robot_API.Controllers.Handling
{
    [ApiController]
    public abstract class SessionControllerBase(IServiceProvider serviceProvider) : ControllerBase
    {
        protected readonly IServiceProvider serviceProvider = serviceProvider;

        protected abstract string HandlingMode { get; }

        [HttpGet]
        public ActionResult<IEnumerable<SessionGetDTO>> GetSessionAll()
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var sessionDTOs = SessionDatabaseHandler.GetSessions(realm, HandlingMode)
                .Select(s => ToSessionGetDTO(s)).ToList();
            return Ok(sessionDTOs);
        }

        [HttpGet("{sessionId}")]
        public IActionResult GetSession(string sessionId)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var session = SessionDatabaseHandler.GetSession(realm, sessionId, HandlingMode);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(ToSessionGetDTO(session));
        }

        [HttpPost]
        public abstract IActionResult PostSession([FromBody] SessionPostDTO sessionDTO);

        [HttpDelete("{sessionId}")]
        public IActionResult DeleteSession(string sessionId)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var session = SessionDatabaseHandler.GetSession(realm, sessionId, HandlingMode);
            if (session == null)
            {
                return NotFound();
            }

            try
            {
                SessionDatabaseHandler.RemoveSession(realm, sessionId, HandlingMode);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting session: {ex.Message}");
            }
        }

        [HttpGet("{sessionId}/actions")]
        public ActionResult<IEnumerable<ActionGetDTO>> GetActionAll(string sessionId)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var session = SessionDatabaseHandler.GetSession(realm, sessionId, HandlingMode);
            if (session == null)
            {
                return NotFound();
            }

            var actionDTOs = SessionDatabaseHandler.GetSessionActions(realm, sessionId, HandlingMode)
                .Select(a => ToActionGetDTO(a)).ToList();
            return Ok(actionDTOs);
        }

        [HttpPost("{sessionId}/actions")]
        public abstract IActionResult PostAction(string sessionId, [FromBody] ActionPostDTO actionDTO);

        [HttpDelete("{sessionId}/actions/{actionId}")]
        public IActionResult DeleteAction(string sessionId, string actionId)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var session = SessionDatabaseHandler.GetSession(realm, sessionId, HandlingMode);
            if (session == null)
            {
                return NotFound("Session not found.");
            }

            var action = SessionDatabaseHandler.GetSessionAction(realm, sessionId, actionId, HandlingMode);
            if (action == null)
            {
                return NotFound("Action not found.");
            }

            try
            {
                SessionDatabaseHandler.RemoveSessionAction(realm, sessionId, actionId, HandlingMode);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting action: {ex.Message}");
            }
        }

        protected static SessionGetDTO ToSessionGetDTO(SessionModel session)
        {
            return new SessionGetDTO
            {
                Id = session.Id.ToString(),
                Mode = session.Mode,
                State = session.State.ToString(),
                StateMessage = session.StateMessage,
                Actions = session.Actions.Select(a => ToActionGetDTO(a)).ToList(),
                CreatedDate = session.CreatedDate,
                ModifiedDate = session.ModifiedDate,
                Processes = session.Processes.Select(p => p.ToString()).ToList(),
                MappingEnabled = session.MappingEnabled,
                MapInputId = session.MapInputId?.ToString(),
                MapOutputName = session.MapOutputName,
                MapOutputId = session.MapOutputId?.ToString(),
            };
        }

        protected static SessionModel FromSessionPostDTO(SessionPostDTO session)
        {
            return new SessionModel
            {
                MappingEnabled = session.MappingEnabled,
                MapInputId = !string.IsNullOrEmpty(session.MapInputId) ? ObjectId.Parse(session.MapInputId) : null,
                MapOutputName = session.MapOutputName,
                MapOutputId = !string.IsNullOrEmpty(session.MapOutputId) ? ObjectId.Parse(session.MapOutputId) : null,
            };
        }

        protected static ActionGetDTO ToActionGetDTO(ActionModel action)
        {
            return new ActionGetDTO
            {
                Id = action.Id.ToString(),
                Command = action.Command.ToString(),
                CreatedDate = action.CreatedDate,
                TargetPosition = action.TargetPosition
            };
        }

        protected static ActionModel FromActionPostDTO(ActionPostDTO action)
        {
            return new ActionModel
            {
                Command = Enum.Parse<ActionCommand>(action.Command),
                TargetPosition = action.TargetPosition
            };
        }
    }
}
