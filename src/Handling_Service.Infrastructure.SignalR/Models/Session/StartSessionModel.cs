using Handling_Service.Core.Session.SessionAggregate;

namespace Handling_Service.Infrastructure.SignalR.Models.Session;

public class StartSessionModel
{
    public HandlingMode HandlingMode { get; set; }
}