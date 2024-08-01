
namespace Leuze_AGV_Handling_Service.WebAPI.Models.Session;

public class ProcessResponseModel(string name, int sessionId, string pid, string state, string createdDate)
{

  public string Name { get; set; } = name;

  public int SessionId { get; set; } = sessionId;

  public string Pid { get; set; } = pid;

  public string State { get; set; } = state;

  public string CreatedDate { get; set; } = createdDate;
  
}
