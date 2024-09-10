namespace Leuze_AGV_Handling_Service.Core.Messages.Services;

public class MessageChannelBase
{
    private bool _enabled = false;
    
    public Task Enable()
    {
        _enabled = true;
        return Task.CompletedTask;
    }

    public Task Disable()
    {
        _enabled = false;
        return Task.CompletedTask;
    }

    public Task<bool> IsEnabled()
    {
        return Task.FromResult(_enabled);
    }
}