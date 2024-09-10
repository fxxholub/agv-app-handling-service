using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Leuze_AGV_Handling_Service.WebAPI.Controllers;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/rest/health")]
[ApiController]
public class HealthController() : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
