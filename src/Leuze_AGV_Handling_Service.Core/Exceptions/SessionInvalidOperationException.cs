namespace Leuze_AGV_Handling_Service.Core.Exceptions;

public class SessionInvalidOperationException: InvalidOperationException
{
  public SessionInvalidOperationException()
  {
  }

  public SessionInvalidOperationException(string message)
    : base(message)
  {
  }

  public SessionInvalidOperationException(string message, Exception inner)
    : base(message, inner)
  {
  }
}
