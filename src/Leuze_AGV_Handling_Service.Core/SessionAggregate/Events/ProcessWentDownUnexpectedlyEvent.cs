using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.Core.SessionAggregate.Events;

public class ProcessWentDownUnexpectedlyEvent: DomainEventBase
{
  public Process DownProcess { get; set; }

  public ProcessWentDownUnexpectedlyEvent(Process downProcess)
  {
    DownProcess = downProcess;
  }
}
