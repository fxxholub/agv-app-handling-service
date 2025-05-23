using Ardalis.Result;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.Core.Session.Interfaces;

public interface ISessionExecutorService
{

    public Task<Result> StartSessionAndReserveConnection(string connectionId, HandlingMode handlingMode, bool mapping);

    public Task<Result> EndSessionOfConnection(string connectionId);
    
    public Task<Result> LeaveSessionAndConnection(string connectionId);

    public Task<Result<bool>> IsCurrentConnection(string connectionId);
    
    public HandlingMode? CurrentHandlingMode();
}