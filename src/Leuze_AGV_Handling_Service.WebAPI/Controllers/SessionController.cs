using Ardalis.Result;
using Asp.Versioning;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Delete;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.List;
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;
using Leuze_AGV_Handling_Service.WebAPI.Models.Session;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Leuze_AGV_Handling_Service.WebAPI.Controllers;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/rest/sessions")]
[ApiController]
public class SessionController(IMediator mediator) : ControllerBase
{
    // TODO: only admin can list all
    [HttpGet]
    public async Task<IActionResult> SessionsGetAll()
    {
        var result = await mediator.Send(new ListSessionsQuery());
        
        if (result.IsSuccess)
        {
            var response = result.Value.Select(session => SessionToResponse(session)).ToList(); 
            return Ok(response);
        }
    
        return BadRequest();
    }
    
    [HttpGet("{sessionId:int}")]
    public async Task<IActionResult> SessionsGet(int sessionId)
    {
        var result = await mediator.Send(new GetSessionQuery(sessionId)); 
        
        if (result.IsSuccess)
        {
            var response = SessionToResponse(result);
            return Ok(response);
        }

        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> SessionsPost([FromBody] SessionRequestModel request)
    {
        var command = SessionFromRequest(request);
        var result = await mediator.Send(command);
      
        if (result.IsSuccess)
        {
            var resultEntity = await mediator.Send(new GetSessionQuery(result.Value));
            
            if (resultEntity.IsSuccess)
            {
                var response = SessionToResponse(resultEntity.Value);
                return Ok(response);
            }
        }
        
        return StatusCode(500, $"Error creating session");
    }
    
    [HttpDelete("{sessionId:int}")]
    public async Task<IActionResult> SessionsDelete(int sessionId)
    {
        var result = await mediator.Send(new DeleteSessionCommand(sessionId));

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound();
    }

    private static CreateSessionCommand SessionFromRequest(SessionRequestModel request)
    {
        HandlingMode handlingMode = Enum.Parse<HandlingMode>(request.HandlingMode ?? throw new ArgumentNullException());
        Lifespan lifespan;
        switch (handlingMode)
        {
            case HandlingMode.Autonomous:
                lifespan = Lifespan.Extended;
                break;
            case HandlingMode.Manual:
                lifespan = Lifespan.Exclusive;
                break;
            default:
                throw new Exception($"Handling mode '{request.HandlingMode}' unknown");
        }
        
        return new CreateSessionCommand(
            handlingMode,
            lifespan
        );
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
                process.HostAddr,
                process.UserName,
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
