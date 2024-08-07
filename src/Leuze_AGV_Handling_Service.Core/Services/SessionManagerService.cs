using Ardalis.Result;
using Leuze_AGV_Handling_Service.Core.Interfaces;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;

namespace Leuze_AGV_Handling_Service.Core.Services;

public class SessionManagerService : ISessionManagerService
{
    private Session? _session = null;

    public Result createSession(Session session)
    {
        if (_session is null)
        {
            _session = session;
            return Result.Success();
        }

        return Result.Conflict();
    }

    public Result<Session> getSession()
    {
        if (_session is null)
        {
            return Result.NoContent();
        }

        return Result.Success(_session);
    }
}