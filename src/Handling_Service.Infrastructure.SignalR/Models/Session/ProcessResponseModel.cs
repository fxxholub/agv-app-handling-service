
namespace Handling_Service.Infrastructure.SignalR.Models.Session;

public class ProcessResponseModel(
  string name,
  string? hostName,
  int? sessionId,
  string? errorReason,
  string pid,
  string state,
  string createdDate
  )
{
  public string Name { get; set; } = name;
  public string? HostName { get; set; } = hostName;

  public int? SessionId { get; set; } = sessionId;

  public string? ErrorReason { get; set; } = errorReason;

  public string Pid { get; set; } = pid;

  public string State { get; set; } = state;

  public string CreatedDate { get; set; } = createdDate;
}
