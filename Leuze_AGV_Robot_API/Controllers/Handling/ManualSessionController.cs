using Asp.Versioning;
using Leuze_AGV_Robot_API.Models;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.RealmDB;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using Realms;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Leuze_AGV_Robot_API.Controllers.Handling
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/handling/manual/session")]
    [ApiController]
    public class ManualSessionController(Realm realm) : RealmControllerBase(realm)
    {
        // GET: api/<ManualSessionController>
        [HttpGet]
        public ActionResult<IEnumerable<SessionGetDTO>> Get()
        {
            using var realm = GetRealmInstance();
            var sessionDTOs = realm.All<SessionModel>().ToList().Select(s => ToGetDTO(s)).ToList();
            return Ok(sessionDTOs);
        }

        //GET api/<ManualSessionController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            using var realm = GetRealmInstance();
            var session = realm.Find<SessionModel>(ObjectId.Parse(id));
            if (session == null)
            {
                return NotFound();
            }
            return Ok(ToGetDTO(session));
        }

        // POST api/<ManualSessionController>
        [HttpPost]
        public IActionResult Post([FromBody] SessionPostDTO sessionDTO)
        {
            if (sessionDTO == null)
            {
                return BadRequest("Session data is null.");
            }

            try
            {
                using var realm = GetRealmInstance();
                realm.Write(() =>
                {
                    realm.Add(FromPostDTO(sessionDTO));
                });

                return Ok("Session created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating session: {ex.Message}");
            }
        }

        // PUT api/<ManualSessionController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ManualSessionController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            using var realm = GetRealmInstance();
            var session = realm.Find<SessionModel>(ObjectId.Parse(id));
            if (session == null)
            {
                return NotFound();
            }

            try
            {
                realm.Write(() =>
                {
                    realm.Remove(session);
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting session: {ex.Message}");
            }
        }

        private static SessionGetDTO ToGetDTO(SessionModel session)
        {
            return new SessionGetDTO
            {
                Id =                session.Id.ToString(),
                State =             session.State.ToString(),
                StateMessage =      session.StateMessage,
                CreatedDate =       session.CreatedDate,
                ModifiedDate =      session.ModifiedDate,
                Processes =         session.Processes.Select(p => p.ToString()).ToList(),
                MappingEnabled =    session.MappingEnabled,
                MapInputId =        session.MapInputId.ToString(),
                MapOutputName =     session.MapOutputName,
                MapOutputId =       session.MapOutputId.ToString(),
            };
        }

        private static SessionModel FromPostDTO(SessionPostDTO session)
        {
            return new SessionModel
            {
                MappingEnabled =    session.MappingEnabled,
                MapInputId =        !string.IsNullOrEmpty(session.MapInputId) ? ObjectId.Parse(session.MapInputId) : null,
                MapOutputName =     session.MapOutputName,
                MapOutputId =       !string.IsNullOrEmpty(session.MapInputId) ? ObjectId.Parse(session.MapOutputId) : null,
            };
        }
    }
}
