
using Leuze_AGV_Handling_Service.UseCases.Session.DTOs;

namespace Leuze_AGV_Handling_Service.Infrastructure.SignalR.Models;

public class SessionResponseModel(
  int id,
  string handlingMode,
  bool mappingEnabled,
  string inputMapRef,
  string outputMapRef,
  string outputMapName,
  string? errorReason,
  string state,
  List<ActionResponseModel> actions,
  List<ProcessResponseModel> processes,
  string createdDate)
{

  public int Id { get; set; } = id;

  public string HandlingMode { get; set; } = handlingMode;

  public bool MappingEnabled { get; set; } = mappingEnabled;

  public string InputMapRef { get; set; } = inputMapRef;

  public string OutputMapRef { get; set; } = outputMapRef;

  public string OutputMapName { get; set; } = outputMapName;

  public string? ErrorReason { get; set; } = errorReason;

  public string State { get; set; } = state;
  
  public List<ActionResponseModel> Actions { get; set; } = actions;
  
  public List<ProcessResponseModel> Processes { get; set; } = processes;

  public string CreatedDate { get; set; } = createdDate;

  public static SessionResponseModel ToModel(SessionDto dto)
  {
    return new SessionResponseModel(
      dto.Id,
      dto.HandlingMode.ToString(),
      dto.MappingEnabled,
      dto.InputMapRef ?? "",
      dto.OutputMapRef ?? "",
      dto.OutputMapName ?? "",
      dto.ErrorReason,
      dto.State.ToString(),
      dto.Actions.Select(action => new ActionResponseModel(
        action.Command.ToString(),
        action.SessionId,
        action.CreatedDate.ToString()
      )).ToList(),
      dto.Processes.Select(process => new ProcessResponseModel(
        process.Name,
        process.HostName,
        process.HostAddr,
        process.UserName,
        process.SessionId,
        process.ErrorReason,
        process.Pid,
        process.State.ToString(),
        process.CreatedDate.ToString()
      )).ToList(),
      dto.CreatedDate.ToString()
    );
  }
}
