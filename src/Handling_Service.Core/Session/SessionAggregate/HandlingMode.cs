namespace Handling_Service.Core.Session.SessionAggregate;

public enum HandlingMode
{
    Manual,
    Automatic,
    Linefollow,
    Autonomous,
    Mapping // secondary process mode - could be added to any of the previous ones
}
