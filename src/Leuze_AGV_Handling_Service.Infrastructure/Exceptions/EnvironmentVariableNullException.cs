namespace Leuze_AGV_Handling_Service.Core.Exceptions;

public class EnvironmentVariableNullException: ArgumentNullException
{
  public EnvironmentVariableNullException()
  {
  }

  public EnvironmentVariableNullException(string message)
    : base(message)
  {
  }

  public EnvironmentVariableNullException(string message, Exception inner)
    : base(message, inner)
  {
  }
}
