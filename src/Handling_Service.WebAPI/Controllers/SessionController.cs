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
using Microsoft.AspNetCore.Mvc;

namespace Handling_Service.WebAPI.Controllers;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/handling/rest/sessions")]
[ApiController]
public class SessionController(IMediator mediator) : ControllerBase
{
    // TODO: only admin can list all
    [HttpGet]
    public async Task<IActionResult> SessionsGetAll()
    {
        var result = await mediator.Send(new ListSessionsQuery());

        if (!result.IsSuccess) return BadRequest();
        
        var response = result.Value.Select(session => SessionToResponse(session)).ToList(); 
        return Ok(response);
    }
    
    [HttpGet("{sessionId:int}")]
    public async Task<IActionResult> SessionsGet(int sessionId)
    {
        var result = await mediator.Send(new GetSessionQuery(sessionId));

        if (!result.IsSuccess) return NotFound();
        
        var response = SessionToResponse(result);
        return Ok(response);
    }
    
    // [HttpPost]
    // public async Task<IActionResult> SessionsPost([FromBody] SessionRequestModel request)
    // {
    //     var resultCommand = SessionFromRequest(request);
    //     
    //     if (!resultCommand.IsSuccess) return StatusCode(500, ResultChecker<CreateSessionCommand>.ErrorMessageJson(resultCommand));
    //     
    //     var result = await mediator.Send(resultCommand.Value);
    //
    //     if (!result.IsSuccess) return StatusCode(500, ResultChecker<int>.ErrorMessageJson(result));
    //     
    //     var resultEntity = await mediator.Send(new GetSessionQuery(result.Value));
    //
    //     if (!resultEntity.IsSuccess) return StatusCode(500, ResultChecker<SessionDto>.ErrorMessageJson(resultEntity));
    //     
    //     var response = SessionToResponse(resultEntity.Value);
    //     return Ok(response);
    // }
    
    [HttpDelete("{sessionId:int}")]
    public async Task<IActionResult> SessionsDelete(int sessionId)
    {
        var result = await mediator.Send(new DeleteSessionCommand(sessionId));

        if (!result.IsSuccess)
            return NotFound();

        return NoContent();
    }

    private static Result<CreateSessionCommand> SessionFromRequest(SessionRequestModel request)
    {
        try
        {
            HandlingMode handlingMode =
                Enum.Parse<HandlingMode>(request.HandlingMode ?? throw new ArgumentNullException());

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

            return Result.Success(new CreateSessionCommand(
                handlingMode,
                lifespan
            ));
        }
        catch (ArgumentNullException)
        {
            return Result.Error(new ErrorList(["HandlingMode null."]));
        }
        catch (ArgumentException)
        {
            return Result.Error(new ErrorList(["HandlingMode invalid."]));
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
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
