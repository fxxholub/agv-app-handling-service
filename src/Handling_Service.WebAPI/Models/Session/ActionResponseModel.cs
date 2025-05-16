
namespace Handling_Service.WebAPI.Models.Session;

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
