using Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.End;
using Leuze_AGV_Handling_Service.UseCases.Session.CQRS.GetCurrent;
using MediatR;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Services;

public class SessionOwnershipService: ISessionOwnershipService
{
    private string? _ownerId = null;
    private int? _sessionId = null;
    
    public Task<bool> Reserve(int sessionId, string clientId)
    {
        if (IsReserved().Result) return Task.FromResult(false);
        
        _sessionId = sessionId;
        _ownerId = clientId;
        return Task.FromResult(true);
    }

    public Task<bool> Free(string clientId)
    {
        if (!IsReservedByMe(clientId).Result) return Task.FromResult(false);

        // release the ownership and session
        _ownerId = null;
        _sessionId = null;

        return Task.FromResult(true);
    }

    public Task<bool> IsReservedByMe(string clientId)
    {
        return Task.FromResult(IsReserved().Result && clientId == _ownerId);
    }
    
    public Task<bool> IsReserved()
    {
        return Task.FromResult(_sessionId is not null && _ownerId is not null);
    }

    public Task<bool> IsFree()
    {
        return Task.FromResult(_sessionId is not null && _ownerId is null);
    }

    public Task<bool> IsNone()
    {
        return Task.FromResult(_sessionId is null && _ownerId is null);
    }

    public Task<int?> GetSessionId()
    {
        return Task.FromResult(_sessionId);
    }
}