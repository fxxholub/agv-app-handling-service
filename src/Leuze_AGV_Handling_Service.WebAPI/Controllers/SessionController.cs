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
[Route("api/v{v:apiVersion}/rest/sessions")]
[ApiController]
public class SessionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new ListSessionsQuery());
        if (response.IsSuccess)
        {
            Ok(response.Value);
        }
    
        return BadRequest(response);
    }
    
    [HttpGet("{sessionId:int}")]
    public async Task<IActionResult> GetById(int sessionId)
    {
        var response = await _mediator.Send(new GetSessionQuery(sessionId)); 
        if (response.IsSuccess)
        {
            return Ok(ToResponse(response));
        }

        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SessionRequestModel request)
    {
        var result = await _mediator.Send(FromRequest(request));
      
        try
        {
            // return CreatedAtAction(nameof(GetById), new { sessionId = result.Value });
            return Ok(ToResponse(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating session: {ex.Message}");
        }
    }
    
    [HttpDelete("{sessionId:int}")]
    public async Task<IActionResult> Delete(int sessionId)
    {
        var result = await _mediator.Send(new DeleteSessionCommand(sessionId));

        if (result.IsSuccess)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    private CreateSessionCommand FromRequest(SessionRequestModel request)
    {
      return new CreateSessionCommand(
        Enum.Parse<HandlingMode>(request.HandlingMode ?? throw new ArgumentNullException()),
        request.MappingEnabled,
        request.InputMapRef,
        request.OutputMapRef,
        request.OutputMapName
      );
    }

    private SessionResponseModel ToResponse(Result<SessionDTO> result)
    {
      return new SessionResponseModel(
        result.Value.Id,
        result.Value.HandlingMode.ToString(),
        result.Value.MappingEnabled,
        result.Value.InputMapRef ?? "",
        result.Value.OutputMapRef ?? "",
        result.Value.OutputMapName ?? "",
        result.Value.State.ToString(),
        result.Value.Processes.Select(process => new ProcessResponseModel(
            process.Name,
            process.HostName,
            process.HostAddr,
            process.UserName,
            process.SessionId,
            process.Pid,
            process.State.ToString(),
            process.CreatedDate.ToString()
          )).ToList(),
        result.Value.CreatedDate.ToString()
        );
    }
}
