using System.ComponentModel.Design.Serialization;
using Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

namespace Leuze_AGV_Handling_Service.Core.Messages.Services;

public class MessageForwarderBase: IMessageForwarder
{
    private bool _enabled = false;
    
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