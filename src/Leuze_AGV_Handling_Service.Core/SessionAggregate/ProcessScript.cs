namespace Leuze_AGV_Handling_Service.Core.SessionAggregate;

public record ProcessScript
(
    string Name,
    string HostName,
    string HostAddr,
    string UserName,
    List<string> Commands
);