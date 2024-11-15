using Ardalis.Result;
using Asp.Versioning;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Delete;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.End;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.GetCurrent;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.List;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Start;
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
    
    [HttpGet]
    public async Task<IActionResult> SessionsGetAll()
    {
        var result = await mediator.Send(new ListSessionsQuery());
        
        if (result.IsSuccess)
        {
            var response = result.Value.Select(session => SessionsToResponse(session)).ToList(); 
            return Ok(response);
        }
    
        return BadRequest();
    }
    
    [HttpGet("{sessionId:int}")]
    public async Task<IActionResult> SessionsGetById(int sessionId)
    {
        var result = await mediator.Send(new GetSessionQuery(sessionId)); 
        
        if (result.IsSuccess)
        {
            var response = SessionsToResponse(result);
            return Ok(response);
        }

        return NotFound();
    }
    
    [HttpGet("current")]
    public async Task<IActionResult> SessionsGetCurrent()
    {
        var result = await mediator.Send(new GetCurrentSessionQuery()); 
        
        if (result.IsSuccess)
        {
            var response = SessionsToResponse(result);
            return Ok(response);
        }

        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> SessionsPost([FromBody] SessionRequestModel request)
    {
        var command = SessionsFromRequest(request);
        var result = await mediator.Send(command);
      
        if (result.IsSuccess)
        {
            var resultEntity = await mediator.Send(new GetSessionQuery(result.Value));
            
            if (resultEntity.IsSuccess)
            {
                var response = SessionsToResponse(resultEntity.Value);
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
    
    [HttpPost("{sessionId:int}/actions")]
    public async Task<IActionResult> SessionsActionsPost(int sessionId, [FromBody] ActionRequestModel request)
    {
        Result<int> result;

        // TODO exception catching
        ActionCommand actionCommand = Enum.Parse<ActionCommand>(request.Command ?? throw new ArgumentNullException());
            
        if (actionCommand is ActionCommand.Start)
        {
            result = await mediator.Send(new StartSessionCommand(sessionId));
        }
        else if (actionCommand is ActionCommand.End)
        {
            result = await mediator.Send(new EndSessionCommand(sessionId));
        }
        else
        {
            result = Result.Invalid();
        }
      
        if (result.IsSuccess)
        {
            return Ok();
        }
        
        return StatusCode(500, $"Error creating session");
    }

    private static CreateSessionCommand SessionsFromRequest(SessionRequestModel request)
    {
      return new CreateSessionCommand(
          Enum.Parse<HandlingMode>(request.HandlingMode ?? throw new ArgumentNullException()),
        request.MappingEnabled,
        request.InputMapRef,
        request.OutputMapRef,
        request.OutputMapName
      );
    }

    private static SessionResponseModel SessionsToResponse(Result<SessionDto> result)
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
