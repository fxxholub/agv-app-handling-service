using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Session.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Session.Interfaces;

public interface ISessionExecutorService
{

    public Task<Result> StartSessionAndReserveConnection(int sessionId, string connectionId);

    public Task<Result> EndSessionOfConnection(int sessionId, string connectionId);
    
    public Task<Result> LeaveSessionAndConnection(string connectionId);

    public Task<Result<bool>> IsCurrentConnection(string connectionId);

    public Task<Result<bool>> IsCurrentSession(int sessionId);
}