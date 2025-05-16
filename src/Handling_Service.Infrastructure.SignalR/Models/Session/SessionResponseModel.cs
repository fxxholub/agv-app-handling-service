
using Handling_Service.UseCases.Session.DTOs;

namespace Handling_Service.Infrastructure.SignalR.Models.Session;

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

  public string ErrorReason { get; set; } = errorReason ?? "";

  public string State { get; set; } = state;
  
  public List<ActionResponseModel> Actions { get; set; } = actions;
  
  public List<ProcessResponseModel> Processes { get; set; } = processes;

  public string CreatedDate { get; set; } = createdDate;

  public static SessionResponseModel ToModel(SessionDto dto)
  {
    return new SessionResponseModel(
      dto.Id,
      dto.HandlingMode.ToString(),
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
