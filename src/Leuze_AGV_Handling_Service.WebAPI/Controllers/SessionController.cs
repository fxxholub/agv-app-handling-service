using Ardalis.Result;
using Asp.Versioning;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Create;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Delete;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Get;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.List;
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
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new ListSessionsQuery());
        
        if (result.IsSuccess)
        {
            var response = result.Value.Select(session => ToResponse(session)).ToList(); 
            return Ok(response);
        }
    
        return BadRequest();
    }
    
    [HttpGet("{sessionId:int}")]
    public async Task<IActionResult> GetById(int sessionId)
    {
        var result = await mediator.Send(new GetSessionQuery(sessionId)); 
        
        if (result.IsSuccess)
        {
            var response = ToResponse(result);
            return Ok(response);
        }

        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SessionRequestModel request)
    {
        var command = FromRequest(request);
        var result = await mediator.Send(command);
      
        if (result.IsSuccess)
        {
            var resultEntity = await mediator.Send(new GetSessionQuery(result.Value));
            
            if (resultEntity.IsSuccess)
            {
                var response = ToResponse(resultEntity.Value);
                return Ok(response);
            }
        }
        
        return StatusCode(500, $"Error creating session");
    }
    
    [HttpDelete("{sessionId:int}")]
    public async Task<IActionResult> Delete(int sessionId)
    {
        var result = await mediator.Send(new DeleteSessionCommand(sessionId));

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound();
    }

    private static CreateSessionCommand FromRequest(SessionRequestModel request)
    {
      return new CreateSessionCommand(
        Enum.Parse<HandlingMode>(request.HandlingMode ?? throw new ArgumentNullException()),
        request.MappingEnabled,
        request.InputMapRef,
        request.OutputMapRef,
        request.OutputMapName
      );
    }

    private static SessionResponseModel ToResponse(Result<SessionDTO> result)
    {
      return new SessionResponseModel(
        result.Value.Id,
        result.Value.HandlingMode.ToString(),
        result.Value.MappingEnabled,
        result.Value.InputMapRef ?? "",
        result.Value.OutputMapRef ?? "",
        result.Value.OutputMapName ?? "",
        result.Value.ErrorReason,
        result.Value.State.ToString(),
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
