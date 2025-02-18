using Ardalis.Result;
using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.Core.Session.Interfaces;

public interface ISessionExecutorService
{

    public Task<Result> StartSessionAndReserveConnection(string connectionId, HandlingMode handlingMode);

    public Task<Result> EndSessionOfConnection(string connectionId);
    
    public Task<Result> LeaveSessionAndConnection(string connectionId);

    public Task<Result<bool>> IsCurrentConnection(string connectionId);
}