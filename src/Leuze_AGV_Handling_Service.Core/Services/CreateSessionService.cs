using Ardalis.Result;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Leuze_AGV_Handling_Service.Core.Services;

public class CreateSessionService(
    IRepository<Session> repository,
    IMediator mediator,
    ILogger<DeleteSessionService> logger
) : ICreateSessionService
{
    public async Task<Result<int>> CreateSession(
        HandlingMode handlingMode,
        bool mappingEnabled,
        string? inputMapRef,
        string? outputMapRef,
        string? outputMapName
        )
    {
        logger.LogInformation("Creating Session");
        
        var newSession = new Session(
            handlingMode,
            mappingEnabled,
            inputMapRef,
            outputMapRef,
            outputMapName
            );
        var createdItem = await repository.AddAsync(newSession);
        
        var domainEvent = new SessionCreatedEvent(createdItem.Id);
        await mediator.Publish(domainEvent);
        
        logger.LogInformation("Created Session {sessionId}", createdItem.Id);
        return createdItem.Id;
    }
}