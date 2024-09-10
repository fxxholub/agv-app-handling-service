using System.ComponentModel.Design.Serialization;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.Messages.Services;

public class MessageChannelBase
{
    private bool _enabled = true;
    
    public void Enable()
    {
        _enabled = true;
    }

    public void Disable()
    {
        _enabled = false;
    }

    public bool IsEnabled()
    {
        return _enabled;
    }
}