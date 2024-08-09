using Ardalis.Result;
using Asp.Versioning;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Session;
using Leuze_AGV_Handling_Service.UseCases.Session.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.Delete;
using Leuze_AGV_Handling_Service.UseCases.Session.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.List;
using Leuze_AGV_Handling_Service.WebAPI.Models.Session;
using MediatR;
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
