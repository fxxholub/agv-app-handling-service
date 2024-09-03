using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Interfaces;

public interface ISessionManagerService
{
    public Result createSession(Session session);

    public Result<Session> getSession();
}