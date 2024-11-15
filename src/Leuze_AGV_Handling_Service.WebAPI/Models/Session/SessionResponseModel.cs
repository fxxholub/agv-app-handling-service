
namespace Leuze_AGV_Handling_Service.WebAPI.Models.Session;

public class SessionResponseModel(
  int id,
  string handlingMode,
  string? errorReason,
  string state,
  List<ActionResponseModel> actions,
  List<ProcessResponseModel> processes,
  string createdDate)
{

  public int Id { get; set; } = id;

  public string HandlingMode { get; set; } = handlingMode;

  public string? ErrorReason { get; set; } = errorReason;

  public string State { get; set; } = state;
  
  public List<ActionResponseModel> Actions { get; set; } = actions;
  
  public List<ProcessResponseModel> Processes { get; set; } = processes;

  public string CreatedDate { get; set; } = createdDate;
}
