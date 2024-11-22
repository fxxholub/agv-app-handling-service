
namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models.Session;

public class ActionResponseModel(
  string command,
  int sessionId,
  string createdDate
  )
{
  public string Command { get; set; } = command;

  public int SessionId { get; set; } = sessionId;

  public string CreatedDate { get; set; } = createdDate;
}
