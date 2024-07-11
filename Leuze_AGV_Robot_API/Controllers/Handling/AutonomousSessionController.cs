using Asp.Versioning;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.RealmDB;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Realms;

namespace Leuze_AGV_Robot_API.Controllers.Handling
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/handling/autonomous/sessions")]
    [ApiController]
    public class AutonomousSessionController(IServiceProvider serviceProvider) : SessionControllerBase(serviceProvider)
    {
        [HttpGet]
        public override ActionResult<IEnumerable<SessionGetDTO>> GetSessionAll()
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var sessionDTOs = SessionDatabaseHandler.GetSessions(realm, "AUTONOMOUS")
                .Select(s => ToSessionGetDTO(s)).ToList();
            return Ok(sessionDTOs);
        }

        [HttpPost]
        public override IActionResult PostSession([FromBody] SessionPostDTO sessionDTO)
        {
            if (sessionDTO == null)
            {
                return BadRequest("Session data is null.");
            }

            try
            {
                using var realm = serviceProvider.GetRequiredService<Realm>();
                var session = FromSessionPostDTO(sessionDTO);
                session.Mode = "AUTONOMOUS";
                SessionDatabaseHandler.AddSession(realm, session);

                return Ok("Session created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating session: {ex.Message}");
            }
        }

        [HttpGet("{sessionId}/actions")]
        public ActionResult<IEnumerable<ActionGetDTO>> GetActionAll(string sessionId)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var session = SessionDatabaseHandler.GetSession(realm, sessionId);
            if (session == null)
            {
                return NotFound();
            }

            var actionDTOs = SessionDatabaseHandler.GetSessionActions(session)
                .Select(a => ToActionGetDTO(a)).ToList();
            return Ok(actionDTOs);
        }

        [HttpPost("{sessionId}/actions")]
        public IActionResult PostAction(string sessionId, [FromBody] ActionPostDTO actionDTO)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var session = SessionDatabaseHandler.GetSession(realm, sessionId);
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
                SessionDatabaseHandler.AddSessionAction(realm, session, action);

                return Ok("Action created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating action: {ex.Message}");
            }
        }

        [HttpDelete("{sessionId}/actions/{actionId}")]
        public IActionResult DeleteAction(string sessionId, string actionId)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
            var session = SessionDatabaseHandler.GetSession(realm, sessionId);
            if (session == null)
            {
                return NotFound("Session not found.");
            }

            var action = SessionDatabaseHandler.GetSessionAction(session, actionId);
            if (action == null)
            {
                return NotFound("Action not found.");
            }

            try
            {
                SessionDatabaseHandler.RemoveSessionAction(realm, session, action);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting action: {ex.Message}");
            }
        }
    }
}
