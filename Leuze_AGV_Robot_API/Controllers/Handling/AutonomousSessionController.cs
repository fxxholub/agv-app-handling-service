using Asp.Versioning;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.ProcessHandler;
using Leuze_AGV_Robot_API.RealmDB;
using Leuze_AGV_Robot_API.StateMachine;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Realms;
using System;

namespace Leuze_AGV_Robot_API.Controllers.Handling
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/handling/autonomous/sessions")]
    [ApiController]
    public class AutonomousSessionController(Realm realm) : SessionControllerBase(realm)
    {
        protected override string HandlingMode { get; } = "AUTONOMOUS";

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
                var stateMachine = new AutonomousSessionStateMachine(session.Id.ToString(), realm, HandlingMode);
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
                var stateMachine = new AutonomousSessionStateMachine(sessionId, realm, HandlingMode);
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
