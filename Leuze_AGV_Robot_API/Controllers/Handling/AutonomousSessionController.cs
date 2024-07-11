using Asp.Versioning;
using Leuze_AGV_Robot_API.Models;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.RealmDB;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Leuze_AGV_Robot_API.Controllers.Handling
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/handling/autonomous/session")]
    [ApiController]
    public class AutonomousSessionController : ControllerBase
    {
        // GET: api/<ManualSessionController>
        [HttpGet]
        public ActionResult<IEnumerable<SessionGetDTO>> GetSessionAll()
        {
            using var realm = GetRealmInstance();
            var sessionDTOs = SessionDatabaseHandler.GetAllSessions(realm)
                .Select(s => ToSessionGetDTO(s)).ToList();
            return Ok(sessionDTOs);
        }

        // GET api/<ManualSessionController>/5
        [HttpGet("{sessionId}")]
        public IActionResult GetSession(string sessionId)
        {
            using var realm = GetRealmInstance();
            var session = SessionDatabaseHandler.FindSession(realm, sessionId);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(ToSessionGetDTO(session));
        }

        // POST api/<ManualSessionController>
        [HttpPost]
        public IActionResult PostSession([FromBody] SessionPostDTO sessionDTO)
        {
            if (sessionDTO == null)
            {
                return BadRequest("Session data is null.");
            }

            try
            {
                using var realm = GetRealmInstance();
                var session = FromSessionPostDTO(sessionDTO);
                SessionDatabaseHandler.AddSession(realm, session);

                return Ok("Session created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating session: {ex.Message}");
            }
        }

        // DELETE api/<ManualSessionController>/5
        [HttpDelete("{sessionId}")]
        public IActionResult DeleteSession(string sessionId)
        {
            using var realm = GetRealmInstance();
            var session = SessionDatabaseHandler.FindSession(realm, sessionId);
            if (session == null)
            {
                return NotFound();
            }

            try
            {
                SessionDatabaseHandler.RemoveSession(realm, session);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting session: {ex.Message}");
            }
        }

        // GET: api/<ManualSessionController>
        [HttpGet("{sessionId}/action")]
        public ActionResult<IEnumerable<ActionGetDTO>> GetActionAll(string sessionId)
        {
            using var realm = GetRealmInstance();
            var session = SessionDatabaseHandler.FindSession(realm, sessionId);
            if (session == null)
            {
                return NotFound();
            }

            var actionDTOs = SessionDatabaseHandler.GetActionsFromSession(session)
                .Select(a => ToActionGetDTO(a)).ToList();
            return Ok(actionDTOs);
        }

        // POST api/<ManualSessionController>
        [HttpPost("{sessionId}/action")]
        public IActionResult PostAction(string sessionId, [FromBody] ActionPostDTO actionDTO)
        {
            using var realm = GetRealmInstance();
            var session = SessionDatabaseHandler.FindSession(realm, sessionId);
            if (session == null)
            {
                return NotFound();
            }

            if (actionDTO == null)
            {
                return BadRequest("Action data is null.");
            }

            try
            {
                var action = FromActionPostDTO(actionDTO);
                SessionDatabaseHandler.AddActionToSession(realm, session, action);

                return Ok("Action created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating action: {ex.Message}");
            }
        }

        private static SessionGetDTO ToSessionGetDTO(SessionModel session)
        {
            return new SessionGetDTO
            {
                Id = session.Id.ToString(),
                State = session.State.ToString(),
                StateMessage = session.StateMessage,
                Actions = session.Actions.Select(a => ToActionGetDTO(a)).ToList(),
                CreatedDate = session.CreatedDate,
                ModifiedDate = session.ModifiedDate,
                Processes = session.Processes.Select(p => p.ToString()).ToList(),
                MappingEnabled = session.MappingEnabled,
                MapInputId = session.MapInputId.ToString(),
                MapOutputName = session.MapOutputName,
                MapOutputId = session.MapOutputId.ToString(),
            };
        }

        private static SessionModel FromSessionPostDTO(SessionPostDTO session)
        {
            return new SessionModel
            {
                MappingEnabled = session.MappingEnabled,
                MapInputId = !string.IsNullOrEmpty(session.MapInputId) ? ObjectId.Parse(session.MapInputId) : null,
                MapOutputName = session.MapOutputName,
                MapOutputId = !string.IsNullOrEmpty(session.MapInputId) ? ObjectId.Parse(session.MapOutputId) : null,
            };
        }

        private static ActionGetDTO ToActionGetDTO(ActionModel action)
        {
            return new ActionGetDTO
            {
                Id = action.Id.ToString(),
                Command = action.Command.ToString(),
                CreatedDate = action.CreatedDate,
                TargetPosition = action.TargetPosition
            };
        }

        private static ActionModel FromActionPostDTO(ActionPostDTO action)
        {
            return new ActionModel
            {
                Command = Enum.Parse<ActionCommand>(action.Command),
                TargetPosition = action.TargetPosition
            };
        }

        private Realm GetRealmInstance()
        {
            // This method should return an instance of your Realm database
            return Realm.GetInstance();
        }
    }
}
