using System.ComponentModel.DataAnnotations;

namespace Leuze_AGV_Handling_Service.WebAPI.Models.Session;

public class SessionResponseModel(
  int id,
  string handlingMode,
  bool mappingEnabled,
  string inputMapRef,
  string outputMapRef,
  string outputMapName,
  string state,
  List<ProcessResponseModel> processes,
  string createdDate)
{

  public int Id { get; set; } = id;

  public string HandlingMode { get; set; } = handlingMode;

  public bool MappingEnabled { get; set; } = mappingEnabled;

  public string InputMapRef { get; set; } = inputMapRef;

  public string OutputMapRef { get; set; } = outputMapRef;

  public string OutputMapName { get; set; } = outputMapName;

  public string State { get; set; } = state;
  
  public List<ProcessResponseModel> Processes { get; set; } = processes;

  public string CreatedDate { get; set; } = createdDate;
}
