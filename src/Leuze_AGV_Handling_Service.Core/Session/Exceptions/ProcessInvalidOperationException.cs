namespace Leuze_AGV_Handling_Service.Core.Session.Exceptions;

public class ProcessInvalidOperationException: InvalidOperationException
{
  public ProcessInvalidOperationException()
  {
  }

  public ProcessInvalidOperationException(string message)
    : base(message)
  {
  }

  public ProcessInvalidOperationException(string message, Exception inner)
    : base(message, inner)
  {
  }
}
