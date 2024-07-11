using Asp.Versioning;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.RealmDB;
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
                session.Mode = SessionMode.MANUAL;
                SessionDatabaseHandler.AddSession(realm, session);

                return Ok("Session created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating session: {ex.Message}");
            }
        }
    }
}
