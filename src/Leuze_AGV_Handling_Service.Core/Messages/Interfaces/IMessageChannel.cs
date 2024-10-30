namespace Leuze_AGV_Handling_Service.Core.Messages.Interfaces;

public interface IMessageChannel
{
    public Task Enable();

    public Task Disable();

    public Task<bool> IsEnabled();
}