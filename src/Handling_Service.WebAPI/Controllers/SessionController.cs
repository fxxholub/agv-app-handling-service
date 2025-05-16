using Ardalis.Result;
using Asp.Versioning;
using Handling_Service.Core.Session.SessionAggregate;
using Handling_Service.UseCases.Session.CQRS.CRUD.Create;
using Handling_Service.UseCases.Session.CQRS.CRUD.Delete;
using Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using Handling_Service.UseCases.Session.CQRS.CRUD.List;
using Handling_Service.UseCases.Session.DTOs;
using Handling_Service.WebAPI.Models.Session;
using Handling_Service.WebAPI.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Handling_Service.WebAPI.Controllers;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/handling/rest/sessions")]
[ApiController]
public class SessionController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> SessionsGetAll()
    {
        var result = await mediator.Send(new ListSessionsQuery());

        if (!result.IsSuccess) return BadRequest();
        
        var response = result.Value.Select(session => SessionToResponse(session)).ToList(); 
        return Ok(response);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{sessionId:int}")]
    public async Task<IActionResult> SessionsGet(int sessionId)
    {
        var result = await mediator.Send(new GetSessionQuery(sessionId));

        if (!result.IsSuccess) return NotFound();
        
        var response = SessionToResponse(result);
        return Ok(response);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{sessionId:int}")]
    public async Task<IActionResult> SessionsDelete(int sessionId)
    {
        var result = await mediator.Send(new DeleteSessionCommand(sessionId));

        if (!result.IsSuccess)
            return NotFound();

        return NoContent();
    }

    private static SessionResponseModel SessionToResponse(Result<SessionDto> result)
    {
        return new SessionResponseModel(
            result.Value.Id,
            result.Value.HandlingMode.ToString(),
            result.Value.ErrorReason,
            result.Value.State.ToString(),
            result.Value.Actions.Select(action => new ActionResponseModel(
                action.Command.ToString(),
                action.SessionId,
                action.CreatedDate.ToString()
                )).ToList(),
            result.Value.Processes.Select(process => new ProcessResponseModel(
                process.Name,
                process.HostName,
                process.SessionId,
                process.ErrorReason,
                process.Pid,
                process.State.ToString(),
                process.CreatedDate.ToString()
              )).ToList(),
            result.Value.CreatedDate.ToString()
        );
    }
}
