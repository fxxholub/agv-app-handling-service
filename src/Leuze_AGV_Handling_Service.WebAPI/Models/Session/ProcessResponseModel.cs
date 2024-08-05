
namespace Leuze_AGV_Handling_Service.WebAPI.Models.Session;

public class ProcessResponseModel(
  string name,
  string? hostName,
  string? hostAddr,
  string? userName,
  int? sessionId,
  string pid,
  string state,
  string createdDate
  )
{

  public string Name { get; set; } = name;
  public string? HostName { get; set; } = hostName;
  public string? HostAddr { get; set; } = hostAddr;
  public string? UserName { get; set; } = userName;

  public int? SessionId { get; set; } = sessionId;

  public string Pid { get; set; } = pid;

  public string State { get; set; } = state;

  public string CreatedDate { get; set; } = createdDate;
  
}
