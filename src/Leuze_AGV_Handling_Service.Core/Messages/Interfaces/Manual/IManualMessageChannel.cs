namespace Leuze_AGV_Handling_Service.Core.Messages.Interfaces.Manual;

public interface IManualMessageChannel : 
        IMessageChannel,
        IManualMessageSender,
        IManualMessageReceiver
{
    
}