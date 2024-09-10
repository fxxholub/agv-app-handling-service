namespace Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

public interface IMessageChannel
{
    public void Enable();

    public void Disable();

    public bool IsEnabled();
}