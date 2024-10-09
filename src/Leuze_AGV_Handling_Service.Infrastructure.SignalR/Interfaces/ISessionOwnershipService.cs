namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Interfaces;

public interface ISessionOwnershipService
{
    public Task<bool> Reserve(int sessionId, string clientId);
    
    public Task<bool> Free(string clientId);

    public Task<bool> IsReservedByMe(string clientId);

    public Task<bool> IsReserved();

    public Task<bool> IsFree();

    public Task<bool> IsNone();

    public Task<int?> GetSessionId();
}