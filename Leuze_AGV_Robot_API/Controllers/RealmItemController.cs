using Microsoft.AspNetCore.Mvc;
using Realms;
using Asp.Versioning;
using Leuze_AGV_Robot_API.Models;
using Leuze_AGV_Robot_API.RealmDB;

namespace Leuze_AGV_Robot_API.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/realmitems")]
    [ApiController]
    public class RealmItemsController(Realm realm) : RealmControllerBase(realm)
    {

        // GET: api/v1/realmitems
        [HttpGet]
        public ActionResult<IEnumerable<RealmItemDTO>> GetAll()
        {
            using var realm = GetRealmInstance();
            var items = realm.All<RealmItem>().ToList().Select(item => item.ToDTO()).ToList();
            return Ok(items);
        }

        // GET: api/v1/realmitems/{id}
        //[HttpGet("{id}")]
        //public IActionResult Get(string id)
        //{
        //    var item = _realm.Find<RealmItem>(ObjectId.Parse(id));
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(item);
        //}

        // POST: api/v1/realmitems
        //[httppost]
        //public iactionresult post([frombody] realmitem item)
        //{
        //    _realm.write(() =>
        //    {
        //        _realm.add(item);
        //    });
        //    return createdataction(nameof(get), new { id = item.id.tostring() }, item);
        //}

        // PUT: api/v1/realmitems/{id}
        //HttpPut("{id}")]
        //public IActionResult Put(string id, [FromBody] RealmItem updatedItem)
        //{
        //    var item = _realm.Find<RealmItem>(ObjectId.Parse(id));
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    _realm.Write(() =>
        //    {
        //        item.Assignee = updatedItem.Assignee;
        //        item.Name = updatedItem.Name;
        //        item.Status = updatedItem.Status;
        //    });

        //    return NoContent();
        //}

        // DELETE: api/v1/realmitems/{id}
        //[HttpDelete("{id}")]
        //public IActionResult Delete(string id)
        //{
        //    var item = _realm.Find<RealmItem>(ObjectId.Parse(id));
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    _realm.Write(() =>
        //    {
        //        _realm.Remove(item);
        //    });

        //    return NoContent();
        //}
    }
}
