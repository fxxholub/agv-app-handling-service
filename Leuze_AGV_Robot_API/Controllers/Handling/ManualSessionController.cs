using Asp.Versioning;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.RealmDB;
using Leuze_AGV_Robot_API.StateMachine;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Realms;

namespace Leuze_AGV_Robot_API.Controllers.Handling
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/handling/manual/sessions")]
    [ApiController]
    public class ManualSessionController(IServiceProvider serviceProvider) : SessionControllerBase(serviceProvider)
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
                using var realm = serviceProvider.GetRequiredService<Realm>();
                var session = FromSessionPostDTO(sessionDTO);
                session.Mode = HandlingMode;
                SessionDatabaseHandler.AddSession(realm, session, HandlingMode);

                return Ok("Session created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating session: {ex.Message}");
            }
        }

        [HttpPost("{sessionId}/actions")]
        public override IActionResult PostAction(string sessionId, [FromBody] ActionPostDTO actionDTO)
        {
            using var realm = serviceProvider.GetRequiredService<Realm>();
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
                var newState = ManualSessionStateMachine.ChangeState(session.State, action.Command);
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
