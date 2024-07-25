using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Realms;
using WebAPI.Models.Handling;
using WebAPI.RealmDB;
using WebAPI.StateMachine;

namespace WebAPI.Controllers.Handling
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/handling/manual/sessions")]
    [ApiController]
    public class ManualSessionController(Realm realm) : SessionControllerBase(realm)
    {
        protected override string HandlingMode { get; } = "MANUAL";

        [HttpPost]
        public override IActionResult PostSession([FromBody] SessionPostDTO sessionDTO)
        {
            if (sessionDTO == null)
            {
                return BadRequest("Session data is null.");
            }

            try
            {
                var session = FromSessionPostDTO(sessionDTO);
                session.Mode = HandlingMode;
                SessionDatabaseHandler.AddSession(realm, session, HandlingMode);

                //initial ROS processes start
                var stateMachine = new ManualSessionStateMachine(session.Id.ToString(), realm, HandlingMode);
                var newState = stateMachine.ChangeState(session.State, ActionCommand.START);
                SessionDatabaseHandler.ChangeSessionState(realm, session.Id.ToString(), HandlingMode, newState);

                return CreatedAtAction(nameof(GetSession), new { sessionId = session.Id }, ToSessionGetDTO(session));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating session: {ex.Message}");
            }
        }

        [HttpPost("{sessionId}/actions")]
        public override IActionResult PostAction(string sessionId, [FromBody] ActionPostDTO actionDTO)
        {
            var session = SessionDatabaseHandler.GetSession(realm, sessionId, HandlingMode);
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
                var stateMachine = new ManualSessionStateMachine(sessionId, realm, HandlingMode);
                var newState = stateMachine.ChangeState(session.State, action.Command);
                SessionDatabaseHandler.AddSessionAction(realm, sessionId, action, HandlingMode);
                SessionDatabaseHandler.ChangeSessionState(realm, sessionId, HandlingMode, newState);

                return Ok("Action created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating action: {ex.Message}");
            }
        }
    }
}
