using Asp.Versioning;
using Leuze_AGV_Robot_API.Models;
using Leuze_AGV_Robot_API.Models.Handling;
using Leuze_AGV_Robot_API.RealmDB;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using Realms;

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
        public ActionResult<IEnumerable<SessionDTO>> Get()
        {
            using var realm = GetRealmInstance();
            var items = realm.All<SessionModel>().ToList().Select(item => item.ToDTO()).ToList();
            return Ok(items);
        }

        // GET api/<ManualSessionController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<ManualSessionController>
        [HttpPost]
        public IActionResult Post([FromBody] SessionDTO sessionDTO)
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
                    var sessionModel = new SessionModel
                    {
                        MappingEnabled = sessionDTO.MappingEnabled,
                        MapInputId = sessionDTO.MapInputId,
                        MapOutputName = sessionDTO.MapOutputName,
                        MapOutputId = sessionDTO.MapOutputId
                    };

                    realm.Add(sessionModel);
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
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
